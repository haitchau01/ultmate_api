using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.Reflection;

public class ArrayModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        //1.Kiểm tra kiểu mô hình có phải là kiểu Enumerable(mảng, danh sách, ...) hay không
        if (!bindingContext.ModelMetadata.IsEnumerableType)
        {
            //Nếu không phải kiểu Enumerable, ModelBinder này không thể xử lý,
            bindingContext.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }
        // 2. Lấy giá trị được cung cấp từ ValueProvider dựa trên tên mô hình
        // bindingContext.ValueProvider là nơi cung cấp dữ liệu đầu vào cho quá trình liên kết mô hình (ví dụ: query string, form data...).
        // bindingContext.ModelName là tên của model property mà ModelBinder đang xử lý.
        // GetValue(bindingContext.ModelName) lấy giá trị từ ValueProvider dựa trên tên model.
        // chuyển đổi giá trị lấy được thành chuỗi. Giá trị này thường là một chuỗi từ request HTTP.
        // Ví dụ, nếu bạn gửi một query string như ids=1,2,3, thì bindingContext.ModelName có thể là "ids" và providedValue sẽ là chuỗi "1,2,3".
        var providedValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();
        if (string.IsNullOrEmpty(providedValue))
        {
            //Nếu rỗng, điều đó có nghĩa là không có giá trị nào được cung cấp cho model này trong request.
            //Trong trường hợp này, ModelBinder coi như liên kết thành công với giá trị null (ModelBindingResult.Success(null)).
            //Điều này có nghĩa là nếu không có giá trị nào được gửi cho tham số mảng, nó sẽ được coi là mảng rỗng (hoặc null).
            bindingContext.Result = ModelBindingResult.Success(null); return Task.CompletedTask;
        }

        /*
         * 3. Xác định kiểu phần tử của mảng và lấy TypeConverter tương ứng
         * bindingContext.ModelType là kiểu dữ liệu của model property (ví dụ: Guid[], int[], string[], List<string>, ...).
         * GetTypeInfo().GenericTypeArguments[0] lấy kiểu phần tử bên trong kiểu Enumerable.
         * Ví dụ, nếu ModelType là Guid[] hoặc List<Guid>, thì genericType sẽ là Guid.
         * TypeDescriptor.GetConverter(genericType) lấy một TypeConverter cho genericType.TypeConverter
         * là một class trong .NET Framework cho phép chuyển đổi giữa các kiểu dữ liệu khác nhau,
         * đặc biệt là từ chuỗi sang kiểu khác và ngược lại.Trong trường hợp này, nó sẽ được sử dụng để chuyển đổi các chuỗi(ví dụ: "1", "2", "3")
         * thành các đối tượng kiểu genericType(ví dụ: Guid, int, string, ...).
         */
        var genericType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
        var converter = TypeDescriptor.GetConverter(genericType);

        /*
         * providedValue.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries):
         * Chia chuỗi providedValue thành một mảng các chuỗi con, sử dụng dấu phẩy (,) làm dấu phân cách.
         * StringSplitOptions.RemoveEmptyEntries đảm bảo rằng các chuỗi rỗng trong kết quả phân tách sẽ bị loại bỏ.
         * Ví dụ, nếu providedValue là "1, 2, , 3", thì kết quả sẽ là mảng các chuỗi: ["1", " 2", " ", " 3"] (lưu ý các khoảng trắng).
         * 
         * .Select(x => converter.ConvertFromString(x.Trim())):
         * Áp dụng phép biến đổi cho mỗi chuỗi con trong mảng kết quả từ Split.
         * x.Trim(): Loại bỏ khoảng trắng thừa ở đầu và cuối mỗi chuỗi con.
         * converter.ConvertFromString(x.Trim()): 
         * Sử dụng TypeConverter lấy được ở bước 4 để chuyển đổi chuỗi con đã được trim thành một đối tượng kiểu genericType.
         * Ví dụ, nếu genericType là Guid và chuỗi con là một chuỗi biểu diễn Guid hợp lệ, nó sẽ được chuyển đổi thành một đối tượng Guid.
         * 
         *  Chuyển đổi kết quả của Select (là một IEnumerable) thành một mảng kiểu object[]. 
         *  Mảng này chứa các đối tượng đã được chuyển đổi từ chuỗi, nhưng vẫn ở dạng object
         */
        var objectArray = providedValue.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                       .Select(x => converter.ConvertFromString(x.Trim())).ToArray();


        /*
         * Array.CreateInstance(genericType, objectArray.Length): 
         * Tạo một mảng mới có kiểu phần tử là genericType và kích thước bằng với objectArray.Length.
         * Ví dụ, nếu genericType là Guid và objectArray.Length là 3, nó sẽ tạo ra một mảng Guid[3].
         * Lưu ý rằng tên biến guidArray có thể gây nhầm lẫn vì nó không nhất thiết phải là mảng Guid, 
         * mà là mảng của kiểu genericType(ví dụ, có thể là intArray, stringArray, ... tùy thuộc vào genericType).
         * objectArray.CopyTo(guidArray, 0): Sao chép các phần tử từ objectArray (mảng các đối tượng kiểu object) vào guidArray (mảng kiểu cụ thể genericType).
         * Quá trình này thực hiện việc unboxing (nếu cần) và chuyển đổi kiểu từ object sang genericType.
         * 
         * bindingContext.Model = guidArray;: Đặt bindingContext.Model (mô hình đã được liên kết) thành guidArray.
         * Đây là mảng đã được tạo và chứa các đối tượng đã chuyển đổi.
        */
        var guidArray = Array.CreateInstance(genericType, objectArray.Length);
        objectArray.CopyTo(guidArray, 0);
        bindingContext.Model = guidArray;

        /*
         * ModelBindingResult.Success(bindingContext.Model):
         * Đặt kết quả liên kết mô hình là thành công, và truyền mô hình đã liên kết (bindingContext.Model) vào kết quả.
         * 
         * return Task.CompletedTask;: Trả về một Task đã hoàn thành, báo hiệu rằng quá trình liên kết mô hình đã kết thúc (bất đồng bộ).
         */
        bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
        return Task.CompletedTask;
    }
}

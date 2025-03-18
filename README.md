# FAQ

1. Why throw exception at service layer and not other layer?
- Separation of Concerns.
- Easy to control errors in a single place.
- Controller only needs to handle responses, no need to worry about business.
- Simple repository, easy to reuse.
- Easy to maintain, extend system with middleware and logging.

2. Why should dto use record for declaration?
- Immutability ensures data integrity.
- Value-based equality makes it easy to check for similarities between objects.
- Support init properties
- Short and readable syntax.
- Good support for serialization and deserialization.
- Fit for purpose of DTO
    + DTO is designed to transfer data, not contain business logic: record is an ideal choice because it focuses on data encapsulation and does not support complex methods like classes.
    + Lightweight and efficient: record is optimized for data storage and transfer, minimizing unnecessary overhead.

3. Method Safety and Method Idempotency, why it matters?
- Method Safety:
    + Safe methods can be called multiple times without worrying about causing unintended changes.
    + They are typically used to query data without affecting the system.

- Method Idempotency:
    + Idempotency ensures that requests can be retried without worrying about causing unintended changes (e.g., in cases of network instability or repeated requests).
    + It is particularly useful in distributed systems or when working with APIs.

- Conclusion
    + Method Safety relates to whether a method changes the system state or not.
    + Method Idempotency relates to whether a method can be called multiple times without changing the result compared to calling it once.
    + Both concepts are crucial in API and system design, ensuring consistency and reliability in applications.

4. Why use CreatedAtRoute?
    + RESTful: When creating a new resource, 201 Created is the correct response code.
    + Dynamic URL generation: No need to hardcode URLs, making the API easy to maintain.
    + HATEOAS support: Clients can easily find the URL of new resources.

5. Khi chạy môi trường thực tế ( trên máy chủ), có thể cấu hình port bằng file launchSetting.json không?
    + Không! File launchSettings.json chỉ được sử dụng trong môi trường phát triển (Development) khi chạy ứng dụng bằng Visual Studio hoặc dotnet CLI (dotnet run).
    + Khi triển khai thực tế (Production), file này không có tác dụng.

6. Model bindind ở controller kế thừa Idisposable và implement dispose thì khi hết request có tự động dispose không?
    + Có, nhưng không hoàn toàn tự động!
    + Trong ASP.NET Core, khi một Controller kế thừa IDisposable và implement Dispose(), framework sẽ không tự động gọi Dispose() khi request kết thúc.
    + Tuy nhiên, nếu controller được tạo thông qua Dependency Injection (DI) với scope phù hợp, ASP.NET Core sẽ gọi Dispose() khi vòng đời của controller kết thúc.

7.  Có thể inject một transient service vô một singleton service bằng constructor không và scope của transient là gì?
    + Có. Nhưng không nên sử dụng hành vi này
    + Điều này có thể gây lỗi "capturing scoped dependencies in singleton" hoặc dẫn đến hành vi không mong muốn.
    + Khi inject Transient vào Singleton, chỉ có một instance duy nhất của Transient được tạo và được giữ lại suốt vòng đời ứng dụng. Nói cách khác nó đã biến đổi thành singleton.
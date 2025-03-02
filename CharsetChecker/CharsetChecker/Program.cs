

using MimeKit;
using System.Text;
using UtfUnknown;

var path = @"E:\file_test_eml\testfile.text";

var byteData = File.ReadAllBytes(path);
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var encode = Encoding.GetEncoding("iso-2022-jp").GetString(byteData);
// Thử phát hiện charset tự động
DetectionResult result = CharsetDetector.DetectFromBytes(byteData);

Console.WriteLine(result.Detected);




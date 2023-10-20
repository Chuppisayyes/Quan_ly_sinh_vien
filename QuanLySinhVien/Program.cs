using System;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
// Khai báo lớp User để đại diện cho thông tin của người dùng
public class User
{
    // Thuộc tính Username để lưu trữ tên đăng nhập của người dùng
    public string Username { get; set; }

    // Thuộc tính Password để lưu trữ mật khẩu của người dùng
    public string Password { get; set; }

    // Constructor (Hàm khởi tạo) của lớp User, nhận username và password để tạo một đối tượng User mới
    public User(string username, string password)
    {
        Username = username; // Gán giá trị của username cho thuộc tính Username
        Password = password; // Gán giá trị của password cho thuộc tính Password
    }
}
// Khai báo lớp static BaiTapQuanLySinhVien
public static class BaiTapQuanLySinhVien
{
    // Tạo một danh sách (List) để lưu trữ đối tượng User
    static List<User> users = new List<User>();

    // Phương thức chính (Main) cho ứng dụng
    static void Main(string[] args)
    {
        // Thiết lập mã hóa đầu ra cho Console thành UTF-8 để hỗ trợ tiếng Việt
        Console.OutputEncoding = Encoding.UTF8;

        // Xóa màn hình Console để bắt đầu một phiên làm việc mới
        Console.Clear();

        // Tải danh sách người dùng từ tệp tin lên trong bộ nhớ
        LoadUsersFromFile();

        // Bắt đầu vòng lặp vô hạn cho phép người dùng thực hiện các tùy chọn
        while (true) // Vòng lặp vô hạn để hiển thị menu liên tục.
        {
            Console.Clear(); // Xóa nội dung màn hình console trước khi hiển thị menu tiếp theo.
            Console.WriteLine("Chào mừng bạn đến với hệ thống đăng nhập và tạo tài khoản!");
            Console.WriteLine("1. Đăng nhập");
            Console.WriteLine("2. Tạo tài khoản");
            Console.WriteLine("3. Hiển thị danh sách tài khoản");
            Console.WriteLine("4. Xóa tài khoản");
            Console.WriteLine("5. Thoát");
            Console.Write("Vui lòng chọn một tùy chọn: ");
            string choice = Console.ReadLine(); // Đọc lựa chọn của người dùng.

            switch (choice) // Kiểm tra lựa chọn của người dùng.
            {
                case "1":
                    Login(); // Gọi hàm đăng nhập.
                    break;
                case "2":
                    Register(); // Gọi hàm tạo tài khoản.
                    break;
                case "3":
                    DisplayUserList(); // Gọi hàm hiển thị danh sách tài khoản.
                    break;
                case "4":
                    DeleteUser(); // Gọi hàm xóa tài khoản.
                    break;
                case "5":
                    SaveUsersToFile(); // Gọi hàm lưu thông tin tài khoản và thoát chương trình.
                    Environment.Exit(0); // Thoát chương trình với mã lỗi 0.
                    break;
                default:
                    Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng thử lại.");
                    Console.ReadLine(); // Hiển thị thông báo lựa chọn không hợp lệ và đợi người dùng nhập một phím bất kỳ để tiếp tục.
                    break;
            }
        }
    }
    static void Login()
    {
        // Hiển thị thông báo yêu cầu người dùng nhập tên đăng nhập.
        Console.Write("Nhập Tài khoản (email/mã số sinh viên/điện thoại): ");
        string username = Console.ReadLine(); // Đọc tên đăng nhập từ người dùng.

        // Hiển thị thông báo yêu cầu người dùng nhập mật khẩu.
        Console.Write("Nhập Mật khẩu: ");
        string password = Console.ReadLine(); // Đọc mật khẩu từ người dùng.

        // Tìm tài khoản trong danh sách dựa trên tên đăng nhập (username).
        User user = users.Find(u => u.Username == username);

        // Kiểm tra xem tài khoản có tồn tại và mật khẩu khớp với tài khoản đó hay không.
        if (user != null && user.Password == password)
        {
            // Nếu có khớp, in ra thông báo đăng nhập thành công.
            Console.WriteLine("Đăng nhập thành công!");
            // Gọi hàm TrangChinh() từ lớp QuanLySinhVien.
            QuanLySinhVien.TrangChinh();
        }
        else
        {
            // Nếu không khớp, in ra thông báo đăng nhập thất bại và đợi người dùng nhấn một phím bất kỳ.
            Console.WriteLine("Đăng nhập thất bại. Vui lòng kiểm tra lại tài khoản và mật khẩu.");
            Console.ReadKey();
        }
    }

    static void Register()
    {
        // Hiển thị thông báo yêu cầu người dùng nhập tên đăng nhập (username).
        Console.Write("Nhập Tài khoản (email/mã số sinh viên/điện thoại): ");
        string username = Console.ReadLine(); // Đọc tên đăng nhập từ người dùng.

        // Hiển thị thông báo yêu cầu người dùng nhập mật khẩu.
        Console.Write("Nhập Mật khẩu: ");
        string password = Console.ReadLine(); // Đọc mật khẩu từ người dùng.

        // Kiểm tra tính hợp lệ của mật khẩu và tên đăng nhập.
        if (!IsStrongPassword(password) || !IsValidUsername(username))
        {
            // Nếu không hợp lệ, in ra thông báo lỗi và hướng dẫn người dùng.
            Console.WriteLine("Tạo tài khoản thất bại. Vui lòng kiểm tra lại thông tin đăng ký.");
            Console.WriteLine("** Mật khẩu bắt buộc phải có ký tự đặc biệt, số và chữ in hoa.");
            Console.WriteLine("** Tài khoản phải đúng định dạng email, số điện thoại (10 chữ số) hoặc mã số sinh viên (11 chữ số).");
            Console.WriteLine("========================================================================================");
            Console.ReadKey();
            return;
        }

        if (users.Exists(u => u.Username == username))
        {
            // Nếu tên đăng nhập đã tồn tại trong danh sách tài khoản (biến `users`), in ra thông báo.
            Console.WriteLine("Tài khoản đã tồn tại.");
        }
        else
        {
            // Nếu tên đăng nhập không tồn tại, tiến hành tạo tài khoản mới.

            // Tạo một đối tượng User mới và thêm nó vào danh sách tài khoản (biến `users`).
            users.Add(new User(username, password));

            // In ra thông báo cho người dùng là tài khoản đã được tạo thành công.
            Console.WriteLine("Tạo tài khoản thành công!");

            // Ghi thông tin tài khoản mới vào tệp "users.txt" để lưu trữ.
            using (StreamWriter writer = new StreamWriter("users.txt", true))
            {
                writer.WriteLine($"{username};{password}");
            }
        }
    }
    static void DisplayUserList()
    {
        // Hiển thị danh sách tài khoản đã đăng ký.
        Console.WriteLine("Danh sách tài khoản đã đăng ký:");
        foreach (User user in users)
        {
            // Duyệt qua danh sách tài khoản và in ra thông tin của từng tài khoản.
            Console.WriteLine($"Tài khoản: {user.Username}, Mật khẩu: {user.Password}");
        }
        Console.ReadKey();
    }
    static void DeleteUser()
    {
        // Yêu cầu người dùng nhập tên đăng nhập cần xóa.
        Console.Write("Nhập Tài khoản cần xóa: ");
        string usernameToDelete = Console.ReadLine();

        // Tìm tài khoản cần xóa trong danh sách tài khoản (biến `users`).
        User userToDelete = users.Find(u => u.Username == usernameToDelete);

        if (userToDelete != null)
        {
            // Nếu tài khoản cần xóa tồn tại, loại bỏ nó khỏi danh sách.
            users.Remove(userToDelete);

            // In ra thông báo cho người dùng là tài khoản đã bị xóa.
            Console.WriteLine($"Tài khoản '{usernameToDelete}' đã bị xóa.");

            // Lưu danh sách tài khoản đã cập nhật vào tệp "users.txt".
            SaveUsersToFile();
            Console.ReadKey();
        }
        else
        {
            // Nếu tài khoản cần xóa không tồn tại, in ra thông báo lỗi.
            Console.WriteLine($"Tài khoản '{usernameToDelete}' không tồn tại.");
            Console.ReadKey();
        }
    }
    static bool IsStrongPassword(string password)
    {
        // Hàm kiểm tra xem mật khẩu có đủ mạnh không, theo các yêu cầu sau:
        // - Ít nhất 8 ký tự.
        // - Chứa ít nhất một ký tự đặc biệt, một số và một chữ in hoa.
        return Regex.IsMatch(password, @"^(?=.*[!@#$%^&*()_+{}\[\]:;<>,.?~\\-])(?=.*[0-9])(?=.*[A-Z]).{8,}$");
    }
    static bool IsValidUsername(string username)
    {
        // Hàm kiểm tra tính hợp lệ của tên đăng nhập (username).
        // Nó kiểm tra xem username có đúng định dạng email, số điện thoại (10 chữ số) hoặc mã số sinh viên (11 chữ số) hay không.
        return Regex.IsMatch(username, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$|^\d{10}$|^\d{11}$");
    }
    static void LoadUsersFromFile()
    {
        if (File.Exists("users.txt"))
        {
            // Kiểm tra xem tệp "users.txt" có tồn tại hay không.

            foreach (string line in File.ReadLines("users.txt"))
            {
                // Duyệt qua từng dòng trong tệp "users.txt".

                string[] parts = line.Split(';');
                // Tách dòng thành các phần, sử dụng dấu chấm phẩy (;) để tách.

                if (parts.Length == 2)
                {
                    // Kiểm tra xem dòng có đúng 2 phần (username và password) hay không.

                    string username = parts[0];
                    string password = parts[1];
                    // Lấy thông tin username và password từ các phần đã tách.

                    users.Add(new User(username, password));
                    // Tạo một đối tượng User mới và thêm vào danh sách `users`.
                }
            }
        }
    }
    static void SaveUsersToFile()
    {
        using (StreamWriter writer = new StreamWriter("users.txt"))
        {
            foreach (User user in users)
            {
                writer.WriteLine($"{user.Username};{user.Password}");
            }
        }
    }
}

public class QuanLySinhVien
{
    // Lớp cha QuanLySinhVien

    public class Students
    {
        // Lớp con Students

        // Các thuộc tính để biểu diễn thông tin về sinh viên
        public string MaSoSinhVien { get; set; }
        public string TenSinhVien { get; set; }
        public string GioiTinh { get; set; }
        public string Tuoi { get; set; }
        public string Email { get; set; }
        public double GPA { get; set; }
        public string XepLoai { get; set; }
        public DateTime NgaySinh;
        public string DiaChi;
        public string SoDienThoai;
    }

    public class Courses
    {
        // Lớp con Courses

        // Các thuộc tính để biểu diễn thông tin về môn học
        public string TenMonHoc { get; set; }
        public string MaMonHoc { get; set; }
        public string GiaoVien { get; set; }
        public DateTime NgayBatDau { get; set; }
    }
    public static void TrangChinh()
    {
        // Thiết lập mã hóa cho nhập và xuất ký tự để hỗ trợ các ký tự Unicode
        Console.InputEncoding = Encoding.Unicode;
        Console.OutputEncoding = Encoding.UTF8;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("============================================ QUẢN LÝ SINH VIÊN / MÔN HỌC ============================================");
            Console.WriteLine("\t1. Danh sách môn học");
            Console.WriteLine("\t2. Danh sách sinh viên");
            Console.WriteLine("\t3. Tim Kiếm sinh Viên");
            Console.WriteLine("\t4. Tìm Kiếm môn học");
            Console.WriteLine("\t5. Thêm môn học");
            Console.WriteLine("\t6. Thêm sinh viên");
            Console.WriteLine("\t7. Xóa sinh viên");
            Console.WriteLine("\t8. Cập nhật thông tin sinh viên");
            Console.WriteLine("\t9. Sắp xếp danh sách sinh viên");
            Console.WriteLine("\t0. Thoát chương trình");
            Console.Write("Vui lòng chọn chức năng thực hiện: ");
            string choice = Console.ReadLine();
            Console.WriteLine("========================================================================================");

            switch (choice)
            {
                case "1":
                    HienThiDanhSachMonHoc();
                    break;
                case "2":
                    HienThiDanhSachSinhVien();
                    break;
                case "3":
                    TimKiemSinhVien();
                    break;
                case "4":
                    TimKiemMonHoc();
                    break;
                case "5":
                    ThemMonHoc();
                    break;
                case "6":
                    ThemSinhVien();
                    break;
                case "7":
                    XoaSinhVien();
                    break;
                case "8":
                    CapNhatSinhVien();
                    break;
                case "9":
                    SortStudent();
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng thử lại.");
                    continue;
            }
            Console.ReadKey();
        }
    }
    public static void HienThiDanhSachMonHoc()
    {
        // Xóa nội dung màn hình console để chuẩn bị cho việc hiển thị danh sách môn học
        Console.Clear();

        // Đường dẫn tới tệp văn bản chứa thông tin môn học
        string filePath = "subjects.txt"; // Thay thế đường dẫn này bằng đường dẫn thực tế của tệp văn bản

        // Đọc thông tin môn học từ tệp văn bản và lưu vào danh sách `courses`
        List<Courses> courses = ReadCoursesFromFile(filePath);

        // Hiển thị danh sách môn học lên màn hình console
        LoadingSubjectsList(courses);
    }

    public static void HienThiDanhSachSinhVien()
    {
        // Xóa nội dung màn hình console để chuẩn bị cho việc hiển thị danh sách sinh viên
        Console.Clear();

        // Đường dẫn tới tệp văn bản chứa thông tin sinh viên
        string filePath = "students.txt"; // Thay thế đường dẫn này bằng đường dẫn thực tế của tệp văn bản

        // Đọc thông tin sinh viên từ tệp văn bản và lưu vào danh sách `students`
        List<Students> students = ReadStudentsFromFile(filePath);

        // Hiển thị danh sách sinh viên lên màn hình console
        LoadingStudentsList(students);
    }

    public static void TimKiemSinhVien()
    {
        // Gọi hàm `TimKiem` để tìm kiếm sinh viên trong tệp `students.txt`
        TimKiem("students.txt", "sinh viên");
    }

    public static void TimKiemMonHoc()
    {
        // Gọi hàm `TimKiem` để tìm kiếm môn học trong tệp `subjects.txt`
        TimKiem("subjects.txt", "môn học");
    }

    public static void TimKiem(string filePath, string objectType)
    {
        // Xóa nội dung màn hình console để chuẩn bị cho việc tìm kiếm
        Console.Clear();

        // Xác định các thuộc tính của đối tượng (sinh viên hoặc môn học)
        string[] objectProperties = objectType == "sinh viên" ?
            new string[] { "MSSV", "Tên SV", "Giới Tính", "Tuổi", "Email", "GPA", "Xếp Loại" } :
            new string[] { "Tên môn học", "Giáo viên", "Mã môn học", "Ngày bắt đầu" };

        // Đọc danh sách đối tượng từ tệp tin và chuyển thành danh sách object
        List<object> objects = objectType == "sinh viên" ?
            ReadStudentsFromFile(filePath).Cast<object>().ToList() :
            ReadCoursesFromFile(filePath).Cast<object>().ToList();
        // Hiển thị thông báo và nhập từ khoá tìm kiếm
        Console.WriteLine($"** Dựa trên {string.Join(" or ", objectProperties)}:\nNhập từ khoá tìm kiếm:");
        string keyword = Console.ReadLine().ToLower();
        // Tìm kiếm đối tượng dựa trên từ khoá
        List<object> KetQuaTimKiem = TimKiem(objects, keyword);
        Console.WriteLine("Kết quả tìm kiếm:");
        if (KetQuaTimKiem.Count != 0)
        {
            // Nếu objectType là "sinh viên", hiển thị danh sách sinh viên
            if (objectType == "sinh viên")
                LoadingStudentsList(KetQuaTimKiem.Cast<Students>().ToList());
            // Nếu objectType là "môn học", hiển thị danh sách môn học
            else
                LoadingSubjectsList(KetQuaTimKiem.Cast<Courses>().ToList());
        }
        else
        {
            // Hiển thị thông báo khi không tìm thấy kết quả tìm kiếm
            Console.WriteLine();
            Console.WriteLine($"\t\t Không tìm thấy {objectType}");
        }

        // Hiển thị các tùy chọn cho người dùng
        Console.WriteLine("\t1. Tìm Kiếm Tiếp\n\t2. Quay Lại Trang Chính\n\t3. Thoát\nVui lòng chọn chức năng thực hiện: ");
        string choice = Console.ReadLine();

        // Xử lý lựa chọn của người dùng
        switch (choice)
        {
            case "1":
                if (objectType == "sinh viên")
                    TimKiemSinhVien();
                else if (objectType == "môn học")
                    TimKiemMonHoc();
                break;
            case "2":
                TrangChinh();
                break;
            case "3":
                Environment.Exit(0);
                break;
        }
    }
    // Hàm thực hiện tìm kiếm trong danh sách các đối tượng
    public static List<object> TimKiem(List<object> objects, string keyword)
    {
        return objects.Where(obj =>
        {
            // Duyệt qua tất cả thuộc tính của đối tượng
            foreach (var prop in obj.GetType().GetProperties())
            {
                string propValue = prop.GetValue(obj)?.ToString().ToLower();
                // Kiểm tra xem từ khóa tìm kiếm có xuất hiện trong thuộc tính không
                if (!string.IsNullOrEmpty(propValue) && propValue.Contains(keyword))
                    return true;
            }
            return false;
        }).ToList();
    }
    public static void ThemMonHoc()
    {
        Console.Clear();
        string filePath = "subjects.txt";
        List<Courses> courses = ReadCoursesFromFile(filePath);

        // Hiển thị thông báo và nhập thông tin môn học
        Console.WriteLine("Nhập thông tin môn học:");
        Console.WriteLine("** Vui lòng nhập đúng định dạng **");

        Courses newCourse = new Courses();

        // Nhập tên môn học
        Console.Write("Tên môn học: ");
        newCourse.TenMonHoc = Console.ReadLine();

        // Nhập mã môn học và kiểm tra tính duy nhất
        while (true)
        {
            Console.Write("Mã môn học: ");
            newCourse.MaMonHoc = Console.ReadLine();

            // Kiểm tra xem mã môn học đã tồn tại hoặc trống không
            if (!courses.Any(course => course.MaMonHoc == newCourse.MaMonHoc))
                break;

            Console.WriteLine("Mã môn học đã tồn tại hoặc để trống. Vui lòng nhập lại.");
        }

        // Nhập tên giáo viên
        Console.Write("Tên giáo viên: ");
        newCourse.GiaoVien = Console.ReadLine();
        DateTime ngayBatDau;

        // Nhập và kiểm tra định dạng ngày bắt đầu
        while (true)
        {
            Console.Write("Ngày bắt đầu (yyyy-MM-dd): ");
            if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out ngayBatDau))
                break;
            Console.WriteLine("Ngày không hợp lệ. Vui lòng nhập lại theo định dạng yyyy-MM-dd.");
        }

        newCourse.NgayBatDau = ngayBatDau;

        try
        {
            // Lưu thông tin mới vào tệp văn bản
            using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                sw.WriteLine("TenMonHoc: " + newCourse.TenMonHoc);
                sw.WriteLine("MaMonHoc: " + newCourse.MaMonHoc);
                sw.WriteLine("GiaoVien: " + newCourse.GiaoVien);
                sw.WriteLine("NgayBatDau: " + newCourse.NgayBatDau.ToString("yyyy-MM-dd"));
                sw.WriteLine();
            }
            Console.WriteLine("Thêm môn học thành công.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Lỗi: " + e.Message);
        }

        // Hiển thị menu chức năng tiếp theo
        Console.WriteLine("\t1. Thêm môn học tiếp\n\t2. Xem danh sách môn học\n\t3. Quay Lại Trang Chính\n\t4. Thoát");
        Console.Write("Vui lòng chọn chức năng thực hiện: ");
        string choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                ThemMonHoc();
                break;
            case "2":
                HienThiDanhSachMonHoc();
                break;
            case "3":
                TrangChinh();
                break;
            case "4":
                Environment.Exit(0);
                break;
        }
    }
    public static void ThemSinhVien()
    {
        Console.Clear();
        string filePath = "students.txt";
        List<Students> students = ReadStudentsFromFile(filePath);
        Console.WriteLine("Nhập thông tin sinh viên:");
        Console.WriteLine("**Vui lòng Nhập đúng định dạng");
        Students newStudent = new Students();

        // Nhập Mã số sinh viên và kiểm tra tính duy nhất
        Console.Write("Mã số sinh viên: ");
        while (true)
        {
            newStudent.MaSoSinhVien = Console.ReadLine();
            if (!students.Any(student => student.MaSoSinhVien == newStudent.MaSoSinhVien))
                break;
            Console.WriteLine("Mã số sinh viên đã tồn tại hoặc để trống. Vui lòng nhập lại.");
        }

        // Nhập Tên sinh viên
        Console.Write("Tên sinh viên: ");
        newStudent.TenSinhVien = Console.ReadLine();

        // Nhập Giới tính
        Console.Write("Giới tính: ");
        newStudent.GioiTinh = Console.ReadLine();

        // Nhập Tuổi
        Console.Write("Tuổi: ");
        newStudent.Tuoi = Console.ReadLine();

        // Nhập Email
        Console.Write("Email: ");
        newStudent.Email = Console.ReadLine();

        double gPA;

        // Nhập và kiểm tra GPA
        while (true)
        {
            Console.Write("GPA: ");
            if (double.TryParse(Console.ReadLine(), out gPA) && gPA >= 0 && gPA <= 4)
                break;
            Console.WriteLine("GPA không hợp lệ. Vui lòng nhập lại.");
        }

        newStudent.GPA = Math.Round(gPA, 1);

        // Xác định Xếp loại dựa trên GPA
        if (newStudent.GPA >= 0 && newStudent.GPA < 1)
            newStudent.XepLoai = "Kém";
        else if (newStudent.GPA >= 1 && newStudent.GPA < 2)
            newStudent.XepLoai = "Yếu";
        else if (newStudent.GPA >= 2 && newStudent.GPA < 2.5)
            newStudent.XepLoai = "Trung Bình";
        else if (newStudent.GPA >= 2.5 && newStudent.GPA < 3.2)
            newStudent.XepLoai = "Khá";
        else if (newStudent.GPA >= 3.2 && newStudent.GPA < 3.6)
            newStudent.XepLoai = "Giỏi";
        else
            newStudent.XepLoai = "Xuất sắc";
        try
        {
            // Lưu thông tin mới vào tệp văn bản
            using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                // Ghi thông tin sinh viên vào tệp
                sw.WriteLine("MaSoSinhVien: " + newStudent.MaSoSinhVien);
                sw.WriteLine("TenSinhVien: " + newStudent.TenSinhVien);
                sw.WriteLine("GioiTinh: " + newStudent.GioiTinh);
                sw.WriteLine("Tuoi: " + newStudent.Tuoi);
                sw.WriteLine("Email: " + newStudent.Email);
                sw.WriteLine("GPA: " + newStudent.GPA);
                sw.WriteLine("XepLoai: " + newStudent.XepLoai);
                sw.WriteLine();
            }
            Console.WriteLine("Thêm sinh viên thành công.");
        }
        catch (Exception e)
        {
            // Xử lý ngoại lệ và thông báo lỗi (nếu có)
            Console.WriteLine("Lỗi: " + e.Message);
        }

        // Hiển thị menu lựa chọn tiếp theo
        Console.WriteLine("\t1. Thêm sinh viên tiếp\n\t2. Xem danh sách sinh viên\n\t3. Quay Lại Trang Chính\n\t4. Thoát");
        Console.Write("Vui lòng chọn chức năng thực hiện: ");
        string choice = Console.ReadLine();
        // Xử lý lựa chọn người dùng
        switch (choice)
        {
            case "1":
                // Chọn thêm sinh viên
                ThemSinhVien();
                break;
            case "2":
                // Chọn xem danh sách sinh viên
                HienThiDanhSachSinhVien();
                break;
            case "3":
                // Chọn quay lại trang chính
                TrangChinh();
                break;
            case "4":
                // Chọn thoát chương trình
                Environment.Exit(0);
                break;
        }
    }
    public static void XoaSinhVien()
    {
        Console.Clear();
        string filePath = "students.txt";
        List<Students> students = ReadStudentsFromFile(filePath);
        Console.Write("Nhập ID của sinh viên cần cập nhật: ");
        string studentID = Console.ReadLine();
        Students studentToDelete = students.Find(student => student.MaSoSinhVien == studentID);
        if (studentToDelete == null)
        {
            Console.WriteLine("Không tìm thấy sinh viên với ID đã nhập.");
            Console.ReadKey();
            return;
        }
        // Loại bỏ sinh viên đã tìm thấy khỏi danh sách
        students.Remove(studentToDelete);
        SaveStudentsToFile(filePath, students);
        Console.WriteLine("Xóa sinh viên thành công.");
        Console.ReadKey();

    }
    public static void CapNhatSinhVien()
    {
        Console.Clear();
        string filePath = "students.txt";
        List<Students> students = ReadStudentsFromFile(filePath);
        Console.Write("Nhập ID của sinh viên cần cập nhật: ");
        string studentID = Console.ReadLine();

        Students studentToUpdate = students.Find(student => student.MaSoSinhVien == studentID);

        if (studentToUpdate == null)
        {
            Console.WriteLine("Không tìm thấy sinh viên với ID đã nhập.");
            Console.ReadKey();
            return;
        }

        // Hiển thị thông tin cũ.
        Console.WriteLine("Thông tin cũ của sinh viên:");
        Console.WriteLine($"Mã số sinh viên: {studentToUpdate.MaSoSinhVien}");
        Console.WriteLine($"Tên sinh viên: {studentToUpdate.TenSinhVien}");
        Console.WriteLine($"Giới tính: {studentToUpdate.GioiTinh}");
        Console.WriteLine($"Tuổi: {studentToUpdate.Tuoi}");
        Console.WriteLine($"Email: {studentToUpdate.Email}");
        // Hiển thị các thuộc tính khác cần thiết.
        Console.WriteLine("===============================================================================================================================");

        Console.WriteLine("Nhập thông tin mới cho sinh viên:");

        Console.Write("Tên sinh viên mới: ");
        string newTenSinhVien = Console.ReadLine();
        Console.Write("Giới tính mới: ");
        string newGioiTinh = Console.ReadLine();
        Console.Write("Tuổi mới: ");
        string newTuoi = Console.ReadLine();
        Console.Write("Email mới: ");
        string newEmail = Console.ReadLine();

        // Cập nhật các thuộc tính của studentToUpdate.
        studentToUpdate.TenSinhVien = newTenSinhVien;
        studentToUpdate.GioiTinh = newGioiTinh;
        studentToUpdate.Tuoi = newTuoi;
        studentToUpdate.Email = newEmail;

        // Lưu danh sách sinh viên đã được cập nhật vào tệp.
        SaveStudentsToFile(filePath, students);

        Console.WriteLine("Cập nhật thông tin sinh viên thành công.");
        Console.ReadKey();
    }
    static void SortStudent()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.Unicode;
        Console.Clear();
        string filePath = "students.txt";// Đường dẫn đến tệp văn bản chứa danh sách sinh viên.
        List<Students> students = ReadStudentsFromFile(filePath); // Đọc danh sách sinh viên từ tệp văn bản.
        if (students != null) // Kiểm tra xem danh sách sinh viên có dữ liệu hay không.
        {
            List<Students> sortedStudents = null;
            // Hiển thị menu cho người dùng chọn cách sắp xếp danh sách sinh viên.
            Console.Write("1.Sắp xếp theo tên\n2.Sắp xếp theo mã số sinh viên\n3.Sắp xếp theo GPA\n\tVui Lòng Nhập: ");
            string cachSapXep = Console.ReadLine();
            switch (cachSapXep)
            {
                case "1":
                    // Sắp xếp danh sách sinh viên theo tên và lưu kết quả vào sortedStudents.
                    sortedStudents = students.OrderBy(student => student.TenSinhVien.Split(' ').Last()).ToList();
                    Console.WriteLine("\tDanh sách sinh viên (Theo tên sinh viên):");
                    break;
                case "2":
                    // Sắp xếp danh sách sinh viên theo mã số sinh viên và lưu kết quả vào sortedStudents.
                    sortedStudents = students.OrderBy(student => student.MaSoSinhVien).ToList();
                    Console.WriteLine("\tDanh sách sinh viên (Theo Mã số sinh viên):");
                    break;
                case "3":
                    // Sắp xếp danh sách sinh viên theo GPA và lưu kết quả vào sortedStudents.
                    sortedStudents = students.OrderBy(student => student.GPA).ToList();
                    Console.WriteLine("\tDanh sách sinh viên (Theo GPA):");
                    break;
            }
            LoadingStudentsList(sortedStudents);  // Hiển thị danh sách sinh viên đã sắp xếp.
            Console.WriteLine("\t1. Sort tiếp\n\t2. Quay Lại Trang Chính\n\t3. Thoát"); // Hiển thị menu lựa chọn tiếp theo cho người dùng.
            Console.Write("Vui lòng chọn chức năng thực hiện: ");
            while (true)
            {
                string choice = Console.ReadLine();
                if (choice == "1" || choice == "2" || choice == "3")
                    switch (choice)
                    {
                        case "1":
                            // Tiếp tục sắp xếp.
                            SortStudent();
                            break;
                        case "2":
                            // Quay lại trang chính.
                            TrangChinh();
                            break;
                        case "3":
                            // Thoát chương trình.
                            Environment.Exit(0);
                            break;
                    }
                else Console.WriteLine("Chức năng không tồn tại, vui lòng chọn lại chức năng thực hiện: ");
            }
            Console.ReadLine();
        }
    }
    // Đọc danh sách sinh viên từ tệp văn bản và chuyển chúng thành danh sách đối tượng sinh viên.
    // Phương thức này đọc từ tệp văn bản và biên dịch dữ liệu thành danh sách các đối tượng Sinh viên.
    static List<Students> ReadStudentsFromFile(string filePath)
    {
        List<Students> students = new List<Students>();
        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                Students currentStudent = null;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(':');
                    if (parts.Length == 2)
                    {
                        string key = parts[0].Trim();
                        string value = parts[1].Trim();

                        if (currentStudent == null)
                        {
                            currentStudent = new Students();
                            students.Add(currentStudent);
                        }

                        // Xác định từng thuộc tính của sinh viên dựa trên tên (key).
                        switch (key)
                        {
                            case "MaSoSinhVien":
                                currentStudent.MaSoSinhVien = value;
                                break;
                            case "TenSinhVien":
                                currentStudent.TenSinhVien = value;
                                break;
                            case "GioiTinh":
                                currentStudent.GioiTinh = value;
                                break;
                            case "Tuoi":
                                currentStudent.Tuoi = value;
                                break;
                            case "Email":
                                currentStudent.Email = value;
                                break;
                            case "GPA":
                                if (double.TryParse(value, out double gpa))
                                {
                                    currentStudent.GPA = gpa;
                                }
                                break;
                            case "XepLoai":
                                currentStudent.XepLoai = value;
                                currentStudent = null; // Kết thúc thông tin của sinh viên, sẵn sàng cho sinh viên tiếp theo
                                break;
                        }
                    }
                }
            }
            return students;
        }
        // Xử lý ngoại lệ nếu có lỗi trong quá trình đọc tệp văn bản.
        catch (Exception e)
        {
            Console.WriteLine("Lỗi: " + e.Message);
            return null; // Trả về null nếu có lỗi.
        }
    }
    // Đọc danh sách môn học từ tệp văn bản và biên dịch chúng thành danh sách đối tượng môn học.
    // Phương thức này đọc từ tệp văn bản và biên dịch dữ liệu thành danh sách các đối tượng Môn học.
    static List<Courses> ReadCoursesFromFile(string filePath)
    {
        List<Courses> courses = new List<Courses>();
        try
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                Courses currentCourse = null;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(':');
                    if (parts.Length == 2)
                    {
                        string key = parts[0].Trim();
                        string value = parts[1].Trim();

                        if (currentCourse == null)
                        {
                            currentCourse = new Courses();
                            courses.Add(currentCourse);
                        }

                        // Xác định từng thuộc tính của môn học dựa trên tên (key).
                        switch (key)
                        {
                            case "TenMonHoc":
                                currentCourse.TenMonHoc = value;
                                break;
                            case "MaMonHoc":
                                currentCourse.MaMonHoc = value;
                                break;
                            case "GiaoVien":
                                currentCourse.GiaoVien = value;
                                break;
                            case "NgayBatDau":
                                if (DateTime.TryParse(value, out DateTime ngayBatDau))
                                {
                                    currentCourse.NgayBatDau = ngayBatDau;
                                }
                                else
                                {
                                    Console.WriteLine($"Lỗi: Không thể chuyển đổi ngày {value}.");
                                }
                                currentCourse = null; // Kết thúc thông tin của môn học, sẵn sàng cho môn học tiếp theo
                                break;
                        }
                    }
                }
            }
            return courses;
        }
        // Xử lý ngoại lệ nếu có lỗi trong quá trình đọc tệp văn bản.
        catch (Exception e)
        {
            Console.WriteLine("Lỗi: " + e.Message);
            return null; // Trả về null nếu có lỗi.
        }
    }
    // Phương thức này dùng để hiển thị danh sách sinh viên ra màn hình với định dạng bảng.
    static void LoadingStudentsList(List<Students> students)
    {
        // In tiêu đề của bảng danh sách sinh viên
        Console.WriteLine();
        Console.WriteLine("{0,-25} {1,-35} {2,-25} {3,-25} {4,-35} {5,-15} {6,-20}", "Mã số sinh viên", "Tên sinh viên", "Giới tính", "Tuổi", "Email", "GPA", "Xếp Loại");
        // Vẽ đường kẻ ngăn cách tiêu đề với dữ liệu
        Console.WriteLine("==================================================================================================================================================================================");

        // Duyệt danh sách sinh viên và hiển thị thông tin của từng sinh viên
        foreach (var student in students)
        {
            // Sử dụng các định dạng cố định để căn chỉnh và hiển thị thông tin của từng sinh viên
            Console.WriteLine("{0,-25} {1,-35} {2,-25} {3,-25} {4,-35} {5,-15} {6,-20}", student.MaSoSinhVien, student.TenSinhVien, student.GioiTinh, student.Tuoi, student.Email, student.GPA, student.XepLoai);
        }

        // Vẽ đường kẻ ngăn cách giữa các sinh viên
        Console.WriteLine("==================================================================================================================================================================================");
    }

    // Phương thức này dùng để hiển thị danh sách môn học ra màn hình với định dạng bảng.
    static void LoadingSubjectsList(List<Courses> courses)
    {
        // In tiêu đề của bảng danh sách môn học
        Console.WriteLine();
        Console.WriteLine("{0,-40} {1,-25} {2,-30} {3,-15}", "Tên môn học", "Mã môn học", "Giáo viên", "Ngày bắt đầu");
        // Vẽ đường kẻ ngăn cách tiêu đề với dữ liệu
        Console.WriteLine("=====================================================================================================================");

        // Duyệt danh sách môn học và hiển thị thông tin của từng môn học
        foreach (var course in courses)
        {
            // Sử dụng các định dạng cố định để căn chỉnh và hiển thị thông tin của từng môn học
            Console.WriteLine("{0,-40} {1,-25} {2,-30} {3,-15}", course.TenMonHoc, course.MaMonHoc, course.GiaoVien, course.NgayBatDau.ToString("dd/MM/yyyy"));
        }

        // Vẽ đường kẻ ngăn cách giữa các môn học
        Console.WriteLine("=====================================================================================================================");
    }

    // Phương thức này dùng để lưu danh sách sinh viên vào tệp văn bản.
    public static void SaveStudentsToFile(string filePath, List<Students> students)
    {
        try
        {
            // Sử dụng StreamWriter để ghi dữ liệu vào tệp văn bản.
            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                foreach (var student in students)
                {
                    // Ghi từng thuộc tính của sinh viên theo định dạng key: value.
                    sw.WriteLine("MaSoSinhVien: " + student.MaSoSinhVien);
                    sw.WriteLine("TenSinhVien: " + student.TenSinhVien);
                    sw.WriteLine("GioiTinh: " + student.GioiTinh);
                    sw.WriteLine("Tuoi: " + student.Tuoi);
                    sw.WriteLine("Email: " + student.Email);
                    sw.WriteLine("GPA: " + student.GPA);
                    sw.WriteLine("XepLoai: " + student.XepLoai);
                    sw.WriteLine();
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Lỗi: " + e.Message);
        }
    }
}
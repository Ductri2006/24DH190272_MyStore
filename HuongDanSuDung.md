# HƯỚNG DẪN CÀI ĐẶT VÀ SỬ DỤNG - 24DH190272_MyStore

## 📋 Mục lục
1. [Yêu cầu hệ thống](#yêu-cầu-hệ-thống)
2. [Hướng dẫn cài đặt](#hướng-dẫn-cài-đặt)
3. [Xử lý lỗi thường gặp](#xử-lý-lỗi-thường-gặp)
4. [Liên hệ](#liên-hệ)

---

## 💻 Yêu cầu hệ thống

Trước khi bắt đầu, đảm bảo máy tính của bạn đã cài đặt:

- ✅ **Visual Studio 2019/2022** (Community Edition trở lên)
- ✅ **SQL Server 2019+** hoặc **SQL Server Express**
- ✅ **.NET Framework 4.7.2** trở lên
- ✅ **Git** (để clone project)

### Tải về các phần mềm cần thiết:
- Visual Studio: https://visualstudio.microsoft.com/downloads/
- SQL Server Express: https://www.microsoft.com/sql-server/sql-server-downloads
- Git: https://git-scm.com/downloads

---

## 📥 HƯỚNG DẪN CÀI ĐẶT

### 🔹 Bước 1: Clone project về máy

#### Cách 1: Dùng Git Bash hoặc Command Prompt

1. Mở **Git Bash** hoặc **Command Prompt**
2. Di chuyển đến thư mục bạn muốn lưu project, ví dụ:
```bash
   cd D:/Projects
```
3. Clone project:
```bash
   git clone https://github.com/Ductri2006/24DH190272_MyStore.git
```
4. Đợi quá trình clone hoàn tất

#### Cách 2: Dùng Visual Studio (Đơn giản hơn)

1. Mở **Visual Studio**
2. Chọn **Clone a repository**
3. Trong mục **Repository location**, paste URL:
```
   https://github.com/Ductri2006/24DH190272_MyStore.git
```
4. Chọn thư mục lưu project
5. Click **Clone**
6. Đợi Visual Studio clone và mở project tự động

---

### 🔹 Bước 2: Tạo Database

Đây là bước **QUAN TRỌNG NHẤT**! Database phải được tạo đúng thì project mới chạy được.

1. **Mở SQL Server Management Studio (SSMS)**

2. **Connect vào SQL Server:**
   - Server name: Thường là `localhost\SQLEXPRESS` hoặc `(localdb)\MSSQLLocalDB`
   - Authentication: **Windows Authentication**
   - Click **Connect**

3. **Mở file SQL script:**
   - Menu **File** → **Open** → **File...**
   - Browse đến thư mục project: `24DH190272_MyStore/Database/MyStore_Script.sql`
   - Click **Open**

4. **Execute script:**
   - Nhấn phím **F5** hoặc click nút **Execute** (▶️)
   - Đợi script chạy xong (có thể mất 5-10 giây)
   - Kiểm tra message: Nếu thấy "Commands completed successfully" = Thành công!

5. **Kiểm tra database đã được tạo:**
   - Trong **Object Explorer**, tìm **Databases**
   - Click chuột phải → **Refresh**
   - Bạn sẽ thấy database **MyStore** xuất hiện
   - Expand **MyStore** → **Tables** → Sẽ thấy các bảng:
     - dbo.Categories
     - dbo.Products
     - dbo.Customers
     - dbo.Orders
     - dbo.OrderDetails

**✅ Nếu thấy đầy đủ các bảng = Bước 2 hoàn thành!**

---

### 🔹 Bước 3: Cấu hình Connection String

Connection String là chuỗi kết nối đến database. **MỖI MÁY KHÁC NHAU** phải sửa khác nhau!

#### 3.1. Xem tên máy tính của bạn:

**Cách 1: Dùng Command Prompt**
```bash
hostname
```
Kết quả sẽ hiện ra tên máy, ví dụ: `LAPTOP-ABC123`

**Cách 2: Xem trong SSMS**
- Khi connect vào SQL Server, tên trong ô **Server name** chính là tên bạn cần
- Ví dụ: `LAPTOP-ABC123\SQLEXPRESS`

#### 3.2. Sửa file Web.config:

1. Trong **Visual Studio**, mở **Solution Explorer**
2. Tìm và mở file **Web.config** (file ở thư mục gốc project)
3. Tìm đoạn `<connectionStrings>` (thường ở dòng 10-15)
4. Sửa phần `data source`:

**TRƯỚC KHI SỬA** (ví dụ):
```xml
<connectionStrings>
  <add name="MyStoreDBEntities" 
       connectionString="...
       data source=DUCTRII-NG\SQLEXPRESS;
       ..." />
</connectionStrings>
```

**SAU KHI SỬA** (thay bằng tên máy của BẠN):
```xml
<connectionStrings>
  <add name="MyStoreDBEntities" 
       connectionString="...
       data source=TÊN_MÁY_CỦA_BẠN\SQLEXPRESS;
       ..." />
</connectionStrings>
```

**Ví dụ cụ thể:**
- Nếu tên máy là `LAPTOP-ABC123`, sửa thành:
```xml
  data source=LAPTOP-ABC123\SQLEXPRESS;
```

- Nếu dùng SQL Server LocalDB:
```xml
  data source=(localdb)\MSSQLLocalDB;
```

5. **Lưu file** (Ctrl + S)

---

### 🔹 Bước 4: Restore NuGet Packages

NuGet Packages là các thư viện cần thiết cho project (Entity Framework, Bootstrap, jQuery...).

1. Trong **Solution Explorer**, **chuột phải vào Solution** (dòng đầu tiên)
2. Chọn **Restore NuGet Packages**
3. Đợi quá trình restore hoàn tất (xem ở thanh trạng thái bên dưới)

**Hoặc dùng Package Manager Console:**
1. Menu **Tools** → **NuGet Package Manager** → **Package Manager Console**
2. Gõ lệnh:
```
   Update-Package -reinstall
```
3. Nhấn Enter và đợi

---

### 🔹 Bước 5: Build Project

1. Menu **Build** → **Build Solution** (hoặc nhấn **Ctrl + Shift + B**)
2. Xem cửa sổ **Output** bên dưới
3. Nếu thấy dòng cuối: **"Build succeeded"** = Thành công! ✅
4. Nếu có lỗi, xem phần [Xử lý lỗi thường gặp](#xử-lý-lỗi-thường-gặp)

---

### 🔹 Bước 6: Chạy Project

1. Nhấn **F5** hoặc click nút **▶️ IIS Express** (màu xanh lá)
2. Trình duyệt sẽ tự động mở với địa chỉ:
```
   http://localhost:52513/
```
   (Port có thể khác tùy máy)

3. **Kiểm tra website:**
   - Trang chủ hiển thị đúng ✅
   - Click vào menu **Sản phẩm** → Thấy danh sách sản phẩm ✅
   - Click **Thêm sản phẩm mới** → Có thể thêm được ✅

**🎉 Nếu tất cả hoạt động bình thường = HOÀN THÀNH!**

---

## ⚠️ XỬ LÝ LỖI THƯỜNG GẶP

### ❌ Lỗi 1: "Login failed for user"

**Thông báo đầy đủ:**
```
Cannot open database "MyStore" requested by the login. 
The login failed for user 'LAPTOP\User'.
```

**Nguyên nhân:**
- Connection string sai tên máy
- SQL Server chưa bật
- Không có quyền truy cập database

**Cách fix:**
1. Kiểm tra lại tên máy trong Web.config
2. Mở **SQL Server Configuration Manager** → Bật **SQL Server Service**
3. Trong SSMS, chuột phải vào database **MyStore** → **Properties** → **Permissions** → Thêm user hiện tại

---

### ❌ Lỗi 2: "Cannot open database 'MyStore'"

**Nguyên nhân:**
- Chưa execute file SQL script
- Tên database sai

**Cách fix:**
1. Mở SSMS
2. Chuột phải **Databases** → **Refresh**
3. Nếu không thấy **MyStore**, quay lại [Bước 2](#bước-2-tạo-database) và execute lại file SQL
4. Kiểm tra tên database trong Web.config có đúng là `MyStore` không

---

### ❌ Lỗi 3: "Could not load file or assembly 'EntityFramework'"

**Nguyên nhân:**
- Thiếu NuGet packages

**Cách fix:**
1. Mở **Package Manager Console**: Tools → NuGet Package Manager → Package Manager Console
2. Gõ lệnh:
```
   Install-Package EntityFramework
```
3. Hoặc:
```
   Update-Package -reinstall
```

---

### ❌ Lỗi 4: Build Failed với nhiều lỗi đỏ

**Cách fix:**
1. Menu **Build** → **Clean Solution**
2. Đợi xong, **Build** → **Rebuild Solution**
3. Nếu vẫn lỗi, đóng Visual Studio
4. Xóa 2 folder: `bin` và `obj` trong thư mục project
5. Mở lại Visual Studio và Build lại

---

### ❌ Lỗi 5: Port bị chiếm (Port already in use)

**Nguyên nhân:**
- Port 52513 đã được ứng dụng khác sử dụng

**Cách fix:**
1. Chuột phải vào **project** (không phải Solution) → **Properties**
2. Tab **Web** bên trái
3. Trong **Servers**, tìm **Project Url**
4. Đổi port khác, ví dụ: `http://localhost:52514/`
5. Click **Create Virtual Directory**
6. Save và chạy lại (F5)

---

### ❌ Lỗi 6: Trang web hiển thị nhưng không có dữ liệu

**Nguyên nhân:**
- File SQL chưa có dữ liệu
- Connection string không đúng

**Cách fix:**
1. Mở SSMS
2. Connect vào SQL Server
3. Expand **MyStore** → **Tables**
4. Chuột phải vào **dbo.Products** → **Select Top 1000 Rows**
5. Nếu không có dữ liệu → Execute lại file SQL script
6. Nếu có dữ liệu nhưng web không hiển thị → Kiểm tra lại Connection String

---

## 🎯 Tính năng chính của hệ thống

### 1. Quản lý Danh mục (Categories)
- Xem danh sách danh mục
- Thêm danh mục mới
- Sửa thông tin danh mục
- Xóa danh mục

### 2. Quản lý Sản phẩm (Products)
- Xem danh sách sản phẩm
- Thêm sản phẩm mới (tên, giá, hình ảnh, mô tả)
- Sửa thông tin sản phẩm
- Xóa sản phẩm
- Tìm kiếm sản phẩm

### 3. Giao diện người dùng
- Hiển thị danh sách sản phẩm với hình ảnh
- Xem chi tiết sản phẩm
- Responsive design (tương thích mobile)

---

## 📊 Cấu trúc Database

### Bảng Categories (Danh mục)
- `IDCate` (int, Primary Key): ID danh mục
- `NameCate` (nvarchar): Tên danh mục

### Bảng Products (Sản phẩm)
- `ProductID` (int, Primary Key, Auto Increment): ID sản phẩm
- `NamePro` (nvarchar): Tên sản phẩm
- `DecriptionPro` (nvarchar): Mô tả chi tiết
- `IDCate` (int, Foreign Key): ID danh mục
- `Price` (decimal): Giá sản phẩm
- `ImagePro` (nvarchar): Đường dẫn hình ảnh

### Bảng Customers (Khách hàng)
- `CustomerID` (int, Primary Key)
- `NameCus` (nvarchar): Tên khách hàng
- `PhoneCus` (nvarchar): Số điện thoại
- `EmailCus` (nvarchar): Email

### Bảng Orders (Đơn hàng)
- `OrderID` (int, Primary Key)
- `OrderDate` (datetime): Ngày đặt hàng
- `CustomerID` (int, Foreign Key): ID khách hàng
- `TotalAmount` (decimal): Tổng tiền

### Bảng OrderDetails (Chi tiết đơn hàng)
- `OrderDetailID` (int, Primary Key)
- `OrderID` (int, Foreign Key): ID đơn hàng
- `ProductID` (int, Foreign Key): ID sản phẩm
- `Quantity` (int): Số lượng
- `UnitPrice` (decimal): Đơn giá

---

## 📞 Liên hệ & Hỗ trợ

### Khi gặp vấn đề:

1. **Đọc kỹ hướng dẫn** và phần xử lý lỗi ở trên
2. **Kiểm tra lại từng bước** đã làm đúng chưa
3. **Chụp màn hình lỗi** và gửi cho nhóm trưởng
4. **Hỏi trong group chat** nhóm

### Thông tin liên hệ:

- **GitHub Repository:** https://github.com/Ductri2006/24DH190272_MyStore
- **Nhóm trưởng:** [Tên bạn]
- **Email nhóm:** [Email của bạn]

---

## 📚 Tài liệu tham khảo

- [ASP.NET MVC Documentation](https://docs.microsoft.com/en-us/aspnet/mvc/)
- [Entity Framework Documentation](https://docs.microsoft.com/en-us/ef/)
- [SQL Server Documentation](https://docs.microsoft.com/en-us/sql/)

---

## 📝 Ghi chú quan trọng

⚠️ **KHÔNG ĐƯỢC**:
- Push file có mật khẩu hoặc thông tin nhạy cảm lên GitHub
- Sửa file SQL script gốc (trừ khi được nhóm trưởng đồng ý)
- Xóa bất kỳ file nào trong project mà không hiểu rõ tác dụng

✅ **NÊN LÀM**:
- Thường xuyên pull code mới nhất từ GitHub: `git pull origin master`
- Commit code thường xuyên với message rõ ràng
- Test kỹ trước khi push code lên GitHub
- Báo với nhóm trước khi thay đổi cấu trúc database

---

## 🎓 Lời kết

Chúc các bạn cài đặt thành công! Nếu gặp khó khăn, đừng ngần ngại hỏi nhóm nhé! 💪

**Cập nhật lần cuối:** 18/11/2025

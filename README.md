# Clean Architecture

## Libraries Used in the Project
- **EntityFrameworkCore**
- **EntityFrameworkCore.Identity**
- **MediatR**
- **AutoMapper**
- **FluentValidation**
- **TS.Result**
- **TS.EntityFrameworkCore.GenericRepository**

docker build -t library_image .

docker run -d -p 9910:9910 --name library_container library_image

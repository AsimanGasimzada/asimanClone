namespace JaleIdentity.Extensions;

public static class FileValidator
{
    public static bool ValidateType(this IFormFile file, string type)
    {
        if (file.ContentType.Contains(type))
        {
            return true;
        }

        return false;
    }


    public static bool ValidateSize(this IFormFile file, int size)
    {
        if(file.Length<size*1024*1024)
            return true;

        return false;

    }
}

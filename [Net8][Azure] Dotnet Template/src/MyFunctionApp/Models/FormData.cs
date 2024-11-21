namespace MyFunctionApp.Models;
public class FormData
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    private char gender;
    public char Gender
    {
        get { return gender; }
        set
        {
            var x = char.ToUpper(value);
            if (x == 'M' || x == 'F')
            {
                gender = x;
            }
            else
            {
                throw new ArgumentException("Gender must be 'M' or 'F'");
            }
        }
    }
}

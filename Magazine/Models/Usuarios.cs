namespace Magazine.Models
{
    public class Usuarios
    {
        

        public int Id {get; set;}
        public string Nombre {get; set;}
        public string Usuario {get; set;}
        public int TipoUsuario {get; set;}
        public string Email {get; set;}
        public string EmailNormalizado {get; set;}
        public string PasswordHash {get; set;}
        public int EmailConfirmado {get; set;}
        public string NombreTipo { get; set;}
    }
}

namespace Reserva_Espectaculo.Helpers
{
    public class ErrorMessage
    {
        public const string Requerido = "El campo {0} es requerido";
        public const string MinInt = "El campo {0} debe contener al menos {1} digitos";
        public const string MaxInt = "El campo {0} debe contener como máximo {1} digitos";
        public const string MinMaxString = "El campo {0} debe contener entre {2} y {1} caracteres";
        public const string DateFormat = "Debe ingresar un formato de fecha válido";
        public const string PhoneFormat = "Debe ingresar un formato de teléfono válido";
        public const string LegajoFormat = "Debe ingresar un formato de legajo válido";
        public const string PositiveNumber = "Debe ingresar un valor positivo";
        public const string PassMissMatch = "La {0} no coincide";
        public const string NotValid = "{0} inválido";
    }
}

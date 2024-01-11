using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System;

namespace BANCO_AMARILLO.Conexion
{
    public class Validaciones
    {
        public bool validacionIp(string ip)
        {
            // Expresión regular para validar una dirección IP en el formato xxx.xxx.xxx.xxx
            string ipAddressPattern = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";

            if (System.Text.RegularExpressions.Regex.IsMatch(ip, ipAddressPattern))
            {
                // Verificar que cada parte esté en el rango correcto (0-255)
                string[] parts = ip.Split('.');
                foreach (string part in parts)
                {
                    if (!int.TryParse(part, out int number) || number < 0 || number > 255)
                    {
                        return false;
                    }
                }
                return true;
            }

            return false;
        }
        public bool NumericoValidacion(string value)
        {
            return int.TryParse(value, out _);
        }

        public IActionResult? ValidateAndReturnMessage(string valueToCheck, Func<string, bool> existCheck)
        {
            if (!NumericoValidacion(valueToCheck))
            {
                return new BadRequestObjectResult("El valor ingresado no es numérico.");
            }

            if (!existCheck(valueToCheck))
            {
                return new BadRequestObjectResult("No existe un registro con el valor proporcionado.");
            }

            return null;
        }
        public string ValidarDireccionMAC(string mac)
        {
            string macAddressPattern = "^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$";

            if (System.Text.RegularExpressions.Regex.IsMatch(mac, macAddressPattern))
            {
                return "La dirección MAC es válida.";
            }
            else
            {
                return "La dirección MAC no es válida.";
            }
        }
        public (bool, string) ValidarNumeroEnteroPositivo(int valor)
        {
            if (valor >= 0)
            {
                return (true, "El valor ingresado es un número entero positivo.");
            }

            return (false, "El valor ingresado no es un número entero positivo.");
        }

        public (bool, string) ValidarNumeroDecimal(string valor)
        {
            if (decimal.TryParse(valor, out _))
            {
                return (true, "El valor ingresado es un número decimal válido.");
            }

            return (false, "El valor ingreso no es un número decimal válido.");
        }

        public (bool, string) ValidarNumeroSerie(string valor)
        {
            // Puedes ajustar la lógica de validación según tus requisitos específicos
            if (!string.IsNullOrEmpty(valor))
            {
                return (true, "El valor ingresado es válido para búsqueda por número de serie.");
            }

            return (false, "El valor ingresado no es válido para búsqueda por número de serie.");
        }

    }
}

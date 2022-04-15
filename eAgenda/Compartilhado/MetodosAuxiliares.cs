using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eAgenda.Compartilhado
{
    public class MetodosAuxiliares
    {
        public static string RecebeHorario(string horario)
        {
            string hora = Console.ReadLine();

            return hora;
        }

   

        public static string ValidarEmail(string mensagem)
        {
            string email = Console.ReadLine();
            string sufixo = "gmail.com";

            return email + sufixo;
        }

        public static string ValidarNumeroTelefone(string mensagem)
        {
            string numero = Console.ReadLine();
            return numero;
        }


        public static string ValidarInputString(string mensagem)
        {
            string palavra;
            while (true)
            {
                Console.Write(mensagem);
                palavra = Console.ReadLine();
                try
                {
                    if (palavra.Length > 0)
                    {
                        return palavra;
                    }
                    else
                    {
                        Console.WriteLine("Digite novamente...");
                        continue;
                    }
                }

                catch
                {
                    Console.WriteLine("Input inválido, tente novamente...");
                }
            }
        }

        public static int ValidarInputInt(string mensagem)
        {
            string numero;
            while (true)
            {
                Console.Write(mensagem);
                numero = Console.ReadLine();
                try
                {
                    if (numero.Length > 0)
                    {
                        return int.Parse(numero);
                    }
                    else
                    {
                        Console.WriteLine("Digite novamente...");
                        continue;
                    }
                }
                catch
                {
                    Console.WriteLine("Input inválido, tente novamente...");
                }
            }
        }

        public static DateTime ValidarInputDate(string mensagem)
        {
            DateTime data;
            while (true)
            {
                Console.Write(mensagem);
                try
                {
                    data = DateTime.Parse(Console.ReadLine());
                    if(data.Day > DateTime.Now.Day)
                        return data;
                    else
                    {
                        Console.WriteLine("Data inválida, digite novamente...");
                    }
                }
                catch
                {
                    Console.WriteLine("Input inválido, tente novamente...");
                }
            }
        }

    }
}

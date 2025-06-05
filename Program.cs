using System;
using System.Collections.Generic;

// Enum para representar o período de empréstimo
public struct PeriodoEmprestimo
{
    public DateTime Inicio;
    public DateTime Fim;
}

// Classe base Pessoa
public class Pessoa
{
    public string Nome { get; set; }
}

// Classe Usuário herda de Pessoa
public class Usuario : Pessoa
{
    public string Matricula { get; set; }
    public List<Livro> LivrosEmprestados { get; set; } = new List<Livro>();
}

// Classe Livro
public class Livro
{
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public string ISBN { get; set; }

    private int paginas;
    public int Paginas
    {
        get { return paginas; }
        set
        {
            if (value < 1)
                throw new ArgumentException("Número de páginas deve ser maior que 0.");
            paginas = value;
        }
    }
}

// Classe Emprestimo
public class Emprestimo
{
    public Usuario Usuario { get; set; }
    public Livro Livro { get; set; }
    public PeriodoEmprestimo Periodo { get; set; }
    public bool Finalizado { get; set; } = false;
}

// Classe principal Biblioteca
public class Biblioteca
{
    private List<Livro> livros = new List<Livro>();
    private List<Usuario> usuarios = new List<Usuario>();
    private List<Emprestimo> emprestimos = new List<Emprestimo>();

    public void AdicionarLivro(Livro livro)
    {
        livros.Add(livro);
    }

    public void AdicionarUsuario(Usuario usuario)
    {
        usuarios.Add(usuario);
    }

    public void RegistrarEmprestimo(string matricula, string tituloLivro)
    {
        var usuario = usuarios.Find(u => u.Matricula == matricula);
        var livro = livros.Find(l => l.Titulo.Trim().ToLower() == tituloLivro.Trim().ToLower());

        if (usuario == null || livro == null)
        {
            Console.WriteLine("Usuário ou livro não encontrado.");
            return;
        }

        // Verificar se o livro já está emprestado
        var emprestado = emprestimos.Exists(e => e.Livro.Titulo == livro.Titulo && !e.Finalizado);
        if (emprestado)
        {
            Console.WriteLine("Este livro já está emprestado e não pode ser emprestado novamente agora.");
            return;
        }

        var emprestimo = new Emprestimo
        {
            Usuario = usuario,
            Livro = livro,
            Periodo = new PeriodoEmprestimo
            {
                Inicio = DateTime.Now,
                Fim = DateTime.Now.AddDays(7)
            }
        };

        usuario.LivrosEmprestados.Add(livro);
        emprestimos.Add(emprestimo);
        Console.WriteLine("Empréstimo registrado com sucesso!");
    }

    public void RegistrarDevolucao(string matricula, string tituloLivro)
    {
        var emprestimo = emprestimos.Find(e => e.Usuario.Matricula == matricula && e.Livro.Titulo == tituloLivro && !e.Finalizado);
        if (emprestimo != null)
        {
            emprestimo.Finalizado = true;
            emprestimo.Usuario.LivrosEmprestados.Remove(emprestimo.Livro);
            Console.WriteLine("Devolução registrada com sucesso!");
        }
        else
        {
            Console.WriteLine("Empréstimo não encontrado.");
        }
    }

    public void ListarLivros()
    {
        foreach (var livro in livros)
        {
            Console.WriteLine($"Título: {livro.Titulo}, Autor: {livro.Autor}, ISBN: {livro.ISBN}, Páginas: {livro.Paginas}");
        }
    }

    public void RelatorioLivrosEmprestados()
    {
        foreach (var emp in emprestimos.FindAll(e => !e.Finalizado))
        {
            Console.WriteLine($"{emp.Livro.Titulo} emprestado para {emp.Usuario.Nome}");
        }
    }

    public void RelatorioUsuariosComLivros()
    {
        foreach (var u in usuarios)
        {
            if (u.LivrosEmprestados.Count > 0)
            {
                Console.WriteLine($"Usuário: {u.Nome}, Livros: {string.Join(", ", u.LivrosEmprestados.ConvertAll(l => l.Titulo))}");
            }
        }
    }
}

// Programa principal
class Program
{
    static void Main()
    {
        Biblioteca biblioteca = new Biblioteca();

        // Cadastro de livros iniciais com número de páginas
        biblioteca.AdicionarLivro(new Livro { Titulo = "Dom Casmurro", Autor = "Machado de Assis", ISBN = "978-85-01-01231-3", Paginas = 256 });
        biblioteca.AdicionarLivro(new Livro { Titulo = "O Cortiço", Autor = "Aluísio Azevedo", ISBN = "978-85-01-04589-2", Paginas = 320 });
        biblioteca.AdicionarLivro(new Livro { Titulo = "Memórias Póstumas", Autor = "Machado de Assis", ISBN = "978-85-01-03456-8", Paginas = 288 });
        biblioteca.AdicionarLivro(new Livro { Titulo = "A Moreninha", Autor = "Joaquim Manuel de Macedo", ISBN = "978-85-01-06543-2", Paginas = 176 });
        biblioteca.AdicionarLivro(new Livro { Titulo = "Iracema", Autor = "José de Alencar", ISBN = "978-85-01-07734-3", Paginas = 192 });

        while (true)
        {
            Console.WriteLine("\nSistema de Biblioteca");
            Console.WriteLine("1 - Cadastrar Livro");
            Console.WriteLine("2 - Cadastrar Usuário");
            Console.WriteLine("3 - Registrar Empréstimo");
            Console.WriteLine("4 - Registrar Devolução");
            Console.WriteLine("5 - Listar Livros");
            Console.WriteLine("6 - Relatório de Livros Emprestados");
            Console.WriteLine("7 - Relatório de Usuários com Livros");
            Console.WriteLine("0 - Sair");
            Console.Write("Escolha uma opção: ");

            string opcao = Console.ReadLine();
            switch (opcao)
            {
                case "1":
                    Console.Write("Título: ");
                    string titulo = Console.ReadLine();
                    Console.Write("Autor: ");
                    string autor = Console.ReadLine();
                    Console.Write("ISBN: ");
                    string isbn = Console.ReadLine();
                    Console.Write("Número de páginas: ");
                    int paginas = int.Parse(Console.ReadLine());
                    biblioteca.AdicionarLivro(new Livro { Titulo = titulo, Autor = autor, ISBN = isbn, Paginas = paginas });
                    break;
                case "2":
                    Console.Write("Nome: ");
                    string nome = Console.ReadLine();
                    Console.Write("Matrícula: ");
                    string matricula = Console.ReadLine();
                    biblioteca.AdicionarUsuario(new Usuario { Nome = nome, Matricula = matricula });
                    break;
                case "3":
                    Console.Write("Matrícula do usuário: ");
                    string matEmp = Console.ReadLine();
                    Console.Write("Título do livro: ");
                    string titEmp = Console.ReadLine();
                    biblioteca.RegistrarEmprestimo(matEmp, titEmp);
                    break;
                case "4":
                    Console.Write("Matrícula do usuário: ");
                    string matDev = Console.ReadLine();
                    Console.Write("Título do livro: ");
                    string titDev = Console.ReadLine();
                    biblioteca.RegistrarDevolucao(matDev, titDev);
                    break;
                case "5":
                    biblioteca.ListarLivros();
                    break;
                case "6":
                    biblioteca.RelatorioLivrosEmprestados();
                    break;
                case "7":
                    biblioteca.RelatorioUsuariosComLivros();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }
    }
}


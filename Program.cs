// Enum para status de empréstimo
enum StatusEmprestimo { Ativo, Finalizado }

// Struct para representar o período do empréstimo
struct PeriodoEmprestimo
{
    public DateTime DataInicio;
    public DateTime DataFim;
}

// Classe base Pessoa
class Pessoa
{
    public string Nome { get; set; }
}

// Usuario herda de Pessoa
class Usuario : Pessoa
{
    public string Matricula { get; set; }
    public List<Emprestimo> Emprestimos { get; set; } = new();
}

// Classe Livro
class Livro
{
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public string ISBN { get; set; }

    private int quantidade;
    public int Quantidade
    {
        get => quantidade;
        set => quantidade = value < 0 ? 0 : value;
    }
}

// Classe Emprestimo
class Emprestimo
{
    public Usuario Usuario { get; set; }
    public Livro Livro { get; set; }
    public PeriodoEmprestimo Periodo { get; set; }
    public StatusEmprestimo Status { get; set; } = StatusEmprestimo.Ativo;
}

// Classe Biblioteca que gerencia tudo
class Biblioteca
{
    public List<Livro> Livros = new();
    public List<Usuario> Usuarios = new();
    public List<Emprestimo> Emprestimos = new();

    public void CadastrarLivro()
    {
        Console.Write("Título: ");
        string titulo = Console.ReadLine();
        Console.Write("Autor: ");
        string autor = Console.ReadLine();
        Console.Write("ISBN: ");
        string isbn = Console.ReadLine();
        Console.Write("Quantidade: ");
        int qtd = int.Parse(Console.ReadLine());

        Livros.Add(new Livro { Titulo = titulo, Autor = autor, ISBN = isbn, Quantidade = qtd });
        Console.WriteLine("Livro cadastrado com sucesso!\n");
    }

    public void CadastrarUsuario()
    {
        Console.Write("Nome: ");
        string nome = Console.ReadLine();
        Console.Write("Matrícula: ");
        string matricula = Console.ReadLine();

        Usuarios.Add(new Usuario { Nome = nome, Matricula = matricula });
        Console.WriteLine("Usuário cadastrado com sucesso!\n");
    }

    public void RegistrarEmprestimo()
    {
        Console.Write("Matrícula do usuário: ");
        string mat = Console.ReadLine();
        Usuario usuario = Usuarios.Find(u => u.Matricula == mat);

        Console.Write("ISBN do livro: ");
        string isbn = Console.ReadLine();
        Livro livro = Livros.Find(l => l.ISBN == isbn);

        if (usuario != null && livro != null && livro.Quantidade > 0)
        {
            livro.Quantidade--;
            PeriodoEmprestimo periodo = new() { DataInicio = DateTime.Today, DataFim = DateTime.Today.AddDays(7) };
            Emprestimo emp = new() { Usuario = usuario, Livro = livro, Periodo = periodo };
            usuario.Emprestimos.Add(emp);
            Emprestimos.Add(emp);
            Console.WriteLine("Empréstimo registrado com sucesso!\n");
        }
        else
        {
            Console.WriteLine("Usuário ou livro não encontrado ou livro indisponível.\n");
        }
    }

    public void RegistrarDevolucao()
    {
        Console.Write("Matrícula do usuário: ");
        string mat = Console.ReadLine();
        Usuario usuario = Usuarios.Find(u => u.Matricula == mat);

        if (usuario != null)
        {
            Emprestimo emp = Emprestimos.Find(e => e.Usuario == usuario && e.Status == StatusEmprestimo.Ativo);
            if (emp != null)
            {
                emp.Status = StatusEmprestimo.Finalizado;
                emp.Livro.Quantidade++;
                Console.WriteLine("Devolução registrada!\n");
            }
            else Console.WriteLine("Nenhum empréstimo ativo encontrado.\n");
        }
        else Console.WriteLine("Usuário não encontrado.\n");
    }

    public void Relatorios()
    {
        Console.WriteLine("\n--- Relatórios ---");
        Console.WriteLine("Livros disponíveis:");
        foreach (var l in Livros) if (l.Quantidade > 0) Console.WriteLine("- " + l.Titulo);

        Console.WriteLine("\nLivros emprestados:");
        foreach (var e in Emprestimos) if (e.Status == StatusEmprestimo.Ativo) Console.WriteLine("- " + e.Livro.Titulo);

        Console.WriteLine("\nUsuários com livros:");
        foreach (var u in Usuarios)
        {
            if (u.Emprestimos.Exists(e => e.Status == StatusEmprestimo.Ativo))
                Console.WriteLine("- " + u.Nome);
        }
        Console.WriteLine();
    }
}

// Classe Program com o menu principal
class Program
{
    static void Main()
    {
        Biblioteca biblio = new();
        int op;
        do
        {
            Console.WriteLine("1 - Cadastrar Livro\n2 - Cadastrar Usuário\n3 - Registrar Empréstimo\n4 - Registrar Devolução\n5 - Exibir Relatórios\n0 - Sair");
            Console.Write("Escolha: ");
            op = int.Parse(Console.ReadLine());
            Console.WriteLine();

            switch (op)
            {
                case 1: biblio.CadastrarLivro(); break;
                case 2: biblio.CadastrarUsuario(); break;
                case 3: biblio.RegistrarEmprestimo(); break;
                case 4: biblio.RegistrarDevolucao(); break;
                case 5: biblio.Relatorios(); break;
                case 0: Console.WriteLine("Saindo..."); break;
                default: Console.WriteLine("Opção inválida!\n"); break;
            }
        } while (op != 0);
    }
}

using SQLite;
using System;
using System.IO;

namespace Projeto_S2B_Main {
	public enum TipoConta { Moeda_Em_Espécie, Cartão_De_Crédito, Cartão_De_Débito, Poupança }
	public enum TipoAtributo { Texto, Numero, Booleano }
	public enum TipoLancamento { Creditar, Debitar }


	/// <summary>
	/// CREATE TABLE Contas (
	///		ID int PRIMARY KEY AUTO_INCREMENT,
	///		Saldo decimal(10,2) NOT NULL,
	///		Tipo ENUM ( 'Moeda_Em_Espécie', 'Cartão_De_Crédito', 'Cartão_De_Débito', 'Poupança' ) NOT NULL,
	///		Nome VARCHAR (200) UNIQUE NOT NULL
	///	);
	/// </summary>
	class Contas {
		public Contas () {
			ID = -1;
		}

		public Contas(decimal saldo, TipoConta tipo, string nome, int id = -1) {
			ID = id;
			Nome = nome;
			Saldo = saldo;
			Tipo = tipo;
		}

        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [NotNull]
        public decimal Saldo { get; set; }

        [NotNull]
        public TipoConta Tipo { get; set; }

        [NotNull, Unique]
        public string Nome { get; set; }
    }

    /// <summary>
    /// CREATE TABLE Categorias (
    ///		ID int PRIMARY KEY AUTO_INCREMENT,
    ///		Grupo VARCHAR(50) NOT NULL,
    ///		Nome VARCHAR(100) UNIQUE NOT NULL
    ///	);
    /// </summary>
    class Categorias {
		public Categorias () {
			ID = -1;
		}

		public Categorias (string nome, string grupo) {
			Nome = nome;
			Grupo = grupo;
		}

		[PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [NotNull]
        public string Grupo { get; set; }

        [NotNull, Unique]
        public string Nome { get; set; }
    }

    /// <summary>
    /// CREATE TABLE Atributos (
    ///		ID int PRIMARY KEY AUTO_INCREMENT,
    ///		ID_categoria int NOT NULL,
    ///		Nome VARCHAR(60) NOT NULL,
    ///		Tipo ENUM ( 'Texto', 'Numero', 'Booleano' ) NOT NULL
    ///	);
    /// </summary>
    class Atributos {
		public Atributos () {
			ID = -1;
		}

		public Atributos (int idCategoria, string nome, TipoAtributo tipo) {
			ID_Categoria = idCategoria;
			Nome = nome;
			Tipo = tipo;
		}
		
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [NotNull]
        public int ID_Categoria { get; set; }

        [NotNull]
        public string Nome { get; set; }

        [NotNull]
        public TipoAtributo Tipo { get; set; }
    }

    /// <summary>
    /// CREATE TABLE Fornecedores (
    ///		ID int PRIMARY KEY AUTO_INCREMENT,
    ///		Nome VARCHAR (150) NOT NULL
    ///	);
    /// </summary>
    class Fornecedores {
        public Fornecedores () {
			ID = -1;
		}

		public Fornecedores (string nome) {
			Nome = nome;
		}

		[PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [NotNull]
        public string Nome { get; set; }
    }

    /// <summary>
    /// CREATE TABLE Lancamentos (
    ///		ID int PRIMARY KEY AUTO_INCREMENT,
    ///		ID_Conta int NOT NULL,
    ///		ID_Fornecedor int NOT NULL,
    ///		ID_Categoria int NOT NULL,
    ///		Valor decimal(10,2) NOT NULL,
    ///		Tipo ENUM ( 'Creditar', 'Debitar' ) NOT NULL,
    ///		Data_Hora DATETIME NOT NULL,
    ///		Comentario VARCHAR(500) NULL
    ///	);
    /// </summary>
    class Lancamentos {
        public Lancamentos () {
			ID = -1;
		}

        public Lancamentos (int idConta, int idFornecedor, int idCategoria, int valor, TipoLancamento tipo, DateTime dataHora, string comentario) {
            ID_Conta = idConta;
            ID_Fornecedor = idFornecedor;
            ID_Categoria = idCategoria;
            Valor = valor;
            Tipo = tipo;
            Data_Hora = dataHora;
            Comentario = comentario;
        }
		
		[PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [NotNull]
        public int ID_Conta { get; set; }

        [NotNull]
        public int ID_Fornecedor { get; set; }

        [NotNull]
        public int ID_Categoria { get; set; }

        [NotNull]
        public decimal Valor { get; set; }

        [NotNull]
        public TipoLancamento Tipo { get; set; }

        [NotNull]
        public DateTime Data_Hora { get; set; }

        public string Comentario { get; set; }
    }

    /// <summary>
    /// CREATE TABLE Lancamento_Atributo (
    ///		ID int PRIMARY KEY AUTO_INCREMENT,
    ///		ID_Lancamento int NOT NULL,
    ///		ID_Atributo int NOT NULL,
    ///		Valor VARCHAR(100) NULL
    ///	);
    /// </summary>
    class Lancamento_Atributo {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [NotNull]
        public int ID_Lancamento { get; set; }

        [NotNull]
        public int ID_Atributo { get; set; }

        [NotNull]
        public string Valor { get; set; }

        public Lancamento_Atributo () { ID = -1; }

        public Lancamento_Atributo (int idLancamento, int idAtributo, string valor) 
        {
            ID_Lancamento = idLancamento;
            ID_Atributo = idAtributo;
            Valor = valor;
        }
    }

	/// <summary>
	/// A classe a seguir deve conter todos os métodos relacionados ao acesso a base de dados.
	/// OBS.: a classe não deve ser instanciada e seus métodos são sempre estáticos.
	/// </summary>
	static class GerenciadorBanco {
		private static SQLiteConnection conn = Connect();

		public static SQLiteConnection Connect () {
			//USE databasePath COMO localhost
			string databasePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

			//CREATE DATABASE financas.sqlite;
			//USE financas.sqlite;
			SQLiteConnection conn = new SQLiteConnection(Path.Combine(databasePath, "financas.sqlite"));

			return conn;
		}

		public static void CloseConnection () {
			conn.Close();
		}

		public static void CreateDatabase () {
			conn.CreateTable<Contas>();
			conn.CreateTable<Categorias>();
			conn.CreateTable<Atributos>();
			conn.CreateTable<Fornecedores>();
			conn.CreateTable<Lancamentos>();
			conn.CreateTable<Lancamento_Atributo>();

			//Registra a categoria padrão se ela ainda não existir
			if (conn.Query<Categorias>("SELECT * FROM Categorias WHERE Nome = 'Indeterminado'").Count == 0) {
				conn.Insert(new Categorias() {
					Grupo = "Outros",
					Nome = "Indeterminado"
				});
			}
		}

		//Contas
		public static int adicionarConta (String nome, decimal saldo, TipoConta tipo) {
			Contas conta = new Contas(saldo, tipo, nome);
			return conn.Insert(conta);
		}

		public static Contas acessarConta (int id) {
			System.Collections.Generic.List<Contas> conta = conn.Query<Contas>(string.Format("Select * from Contas where ID = {0};", id));
			if (conta.Count > 0)
				return conta[0];
			else
				return new Contas();

		}
	
		public static System.Collections.Generic.List<Contas> acessarContas () {
			return conn.Query<Contas>(string.Format("Select * from Contas"));
		}

		public static System.Collections.Generic.List<String> acessarNomeContas () {
			System.Collections.Generic.List<Contas> contas = conn.Query<Contas>(string.Format("Select * from Contas"));
			System.Collections.Generic.List<String> nomes = new System.Collections.Generic.List<string>();
			foreach (Contas i in contas)
				nomes.Add(i.Nome);
			return nomes;

		}

		public static void updateConta (Contas conta) {
			conn.Update(conta);
		}

		public static void deleteConta (Contas conta) {
			conn.Delete(conta);
		}

		public static void deleteConta (object conta_ID) {
			conn.Delete<Contas>(conta_ID);
		}

        //Fornecedor
		public static int adicionarFornecedor (String nome) {
			Fornecedores fornecedor = new Fornecedores(nome);
			return conn.Insert(fornecedor);
		}

		public static Fornecedores acessarFornecedor (int id) {
			System.Collections.Generic.List<Fornecedores> fornecedor = conn.Query<Fornecedores>(string.Format("Select * from Fornecedores where ID = {0};", id));
			if (fornecedor.Count > 0)
				return fornecedor[0];
			else
				return new Fornecedores();
		}

		public static System.Collections.Generic.List<Fornecedores> acessarFornecedores () {
			return conn.Query<Fornecedores>(string.Format("Select * from Fornecedores"));
		}

		public static void updateFornecedor (Fornecedores fornecedor) {
			conn.Update(fornecedor);
		}

        public static void deleteFornecedor (Fornecedores fornecedor) {
            conn.Delete(fornecedor);
        }

        public static void deleteFornecedor (object fornecedor_ID) 
        {
            conn.Delete<Fornecedores>(fornecedor_ID);
        }

        //Categorias
		public static int adicionarCategorias (String nome, String grupo) {
			Categorias categoria = new Categorias(nome, grupo);
			return conn.Insert(categoria);
		}

		public static Categorias acessarCategoria (int id) {
			System.Collections.Generic.List<Categorias> categoria = conn.Query<Categorias>(string.Format("Select * from Categorias where ID = {0};", id));
			if (categoria.Count > 0)
				return categoria[0];
			else
				return new Categorias();

		}

		public static System.Collections.Generic.List<Categorias> acessarCategorias () {
			return conn.Query<Categorias>(string.Format("Select * from Categorias"));
		}

		public static void updateCategoria (Categorias categoria) {
			conn.Update(categoria);
		}

		public static void deleteCategoria (Categorias categoria) {
			conn.Delete(categoria);
		}

        public static void deleteCategoria (object categoria_ID) {
            conn.Delete<Categorias>(categoria_ID);
        }

        //Atributo
        public static int adicionarAtributo (int idCategoria, String nome, TipoAtributo tipo) {
            Atributos atributo = new Atributos(idCategoria, nome, tipo);
            return conn.Insert(atributo);
        }

        public static Atributos acessarAtributo (int id) {
            System.Collections.Generic.List<Atributos> atributo = conn.Query<Atributos>(string.Format("Select * from Atributo where ID = {0};", id));
            if (atributo.Count > 0)
                return atributo[0];
            else
				return new Atributos();
        }

        public static System.Collections.Generic.List<Atributos> acessarAtributo () {
            return conn.Query<Atributos>(string.Format("Select * from Atributo"));
        }

        public static void updateAtributo (Atributos atributo) {
            conn.Update(atributo);
        }

        public static void deleteAtributo (Atributos atributo) {
            conn.Delete(atributo);
        }

        public static void deleteAtributos (object atributo_ID) {
            conn.Delete<Atributos>(atributo_ID);
        }

        //Lançamento
        public static int adicionarLancamento (int idConta, int idFornecedor, int idCategoria, int valor, TipoLancamento Tipo, DateTime dataHora, string comentario){
            Lancamentos lancamento = new Lancamentos(idConta, idFornecedor, idCategoria, valor, Tipo, dataHora, comentario);
            Contas conta = acessarConta(idConta);
            if (conta.ID == -1)
                return -1;
            if (Tipo == TipoLancamento.Creditar)
                conta.Saldo += valor;
            else
                conta.Saldo -= valor;
            updateConta(conta);
            return conn.Insert(lancamento);
        }

        public static Lancamentos acessarLancamento (int id) {
            System.Collections.Generic.List<Lancamentos> lancamento = conn.Query<Lancamentos>(string.Format("Select * from Lancamentos where ID = {0};", id));
            if (lancamento.Count > 0)
                return lancamento[0];
            else
				return new Lancamentos();
        }

        public static System.Collections.Generic.List<Lancamentos> acessarLancamento () {
            return conn.Query<Lancamentos>(string.Format("Select * from Lancamentos"));
        }

        public static void updateLancamento (Lancamentos lancamento) {
            conn.Update(lancamento);
        }

        public static void deleteLancamento (Lancamentos lancamento) {
            conn.Delete(lancamento);
        }

        public static void deleteLancamento (object lancamento_ID) {
            conn.Delete<Lancamentos>(lancamento_ID);
        }

        //Lançamento_Atributo
        public static int adicionarLancamentoAtributo (int idLancamento, int idAtributo, string valor) {
            Lancamentos lancamento = acessarLancamento(idLancamento);
            Atributos atributo = acessarAtributo(idAtributo);
            if (lancamento.ID == -1 || atributo.ID == -1)
                return -1;
            Lancamento_Atributo lancamentoAtributo = new Lancamento_Atributo(idLancamento, idAtributo, valor);
            return conn.Insert(lancamentoAtributo);
        }

        public static Lancamento_Atributo acessarLancamentoAtributo (int id) 
        {
            System.Collections.Generic.List<Lancamento_Atributo> lancamentoAtributo = conn.Query<Lancamento_Atributo>(string.Format("Select * from Lancamento_Atributo where ID = {0};", id));
            if (lancamentoAtributo.Count > 0)
                return lancamentoAtributo[0];
            else
                return new Lancamento_Atributo();
        }

        public static System.Collections.Generic.List<Lancamento_Atributo> acessarLancamentoAtributo () {
            return conn.Query<Lancamento_Atributo>(string.Format("Select * from Lancamento_Atributo"));
        }

        public static void updateLancamentoaAtributo (Lancamento_Atributo lancamentoAtributo) {
            conn.Update(lancamentoAtributo);
        }

        public static void deleteLancamentoAtributo (Lancamento_Atributo lancamentoAtributo) {
            conn.Delete(lancamentoAtributo);
        }

        public static void deletelancamentoAtributo (object lancamentoAtributo_ID) {
            conn.Delete<Lancamento_Atributo>(lancamentoAtributo_ID);
        }
    }
}
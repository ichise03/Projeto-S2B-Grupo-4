using SQLite;
using System;
using System.IO;

namespace Projeto_S2B_Main
{
    /// <summary>
    /// CREATE TABLE Contas (
    ///		ID int PRIMARY KEY AUTO_INCREMENT,
    ///		Saldo decimal(10,2) NOT NULL,
    ///		Tipo ENUM ( 'Moeda_Em_Espécie', 'Cartão_De_Crédito', 'Cartão_De_Débito', 'Poupança' ) NOT NULL,
    ///		Nome VARCHAR (200) UNIQUE NOT NULL
    ///	);
    /// </summary>
    class Contas
    {
		public Contas () { ID = -1; }
		public enum TipoConta { Moeda_Em_Espécie, Cartão_De_Crédito, Cartão_De_Débito, Poupança }

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
    class Categorias
    {
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
    class Atributos
    {
        public enum TipoAtributo { Texto, Numero, Booleano }

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
    class Fornecedores
    {
		public Fornecedores () { ID = -1; }
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
    class Lancamentos
    {
        public enum TipoLancamento { Creditar, Debitar }

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
    class Lancamento_Atributo
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [NotNull]
        public int ID_Lancamento { get; set; }

        [NotNull]
        public int ID_Atributo { get; set; }

        [NotNull]
        public string Valor { get; set; }
    }

    static class SGBD
    {
        public static SQLiteConnection Connect()
        {
            //USE databasePath COMO localhost
            string databasePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

            //CREATE DATABASE financas.sqlite;
            //USE financas.sqlite;
            SQLiteConnection conn = new SQLiteConnection(Path.Combine(databasePath, "financas.sqlite"));

            return conn;
        }

        public static void CreateDatabase()
        {
            SQLiteConnection conn = Connect();

            conn.CreateTable<Contas>();
            conn.CreateTable<Categorias>();
            conn.CreateTable<Atributos>();
            conn.CreateTable<Fornecedores>();
            conn.CreateTable<Lancamentos>();
            conn.CreateTable<Lancamento_Atributo>();

            //Registra a categoria padrão se ela ainda não existir
            if (conn.Query<Categorias>("SELECT * FROM Categorias WHERE Nome = 'Indeterminado'").Count == 0)
            {
                conn.Insert(new Categorias()
                {
                    Grupo = "Outros",
                    Nome = "Indeterminado"
                });
            }

            conn.Close();
        }
    }
	class gerenciadorBanco {

		public int adicionarConta (String nome, decimal saldo, Contas.TipoConta tipo) {
			SQLiteConnection bd = SGBD.Connect();
			Contas conta = new Contas();
			conta.Nome = nome;
			conta.Saldo = saldo;
			conta.Tipo = tipo;
			return bd.Insert(conta);
		}
		public Contas acessarConta (int id) {
			SQLiteConnection bd = SGBD.Connect();
			System.Collections.Generic.List<Contas> conta = bd.Query<Contas>(string.Format("Select * from Contas where ID = {0};", id));
			if (conta.Count > 0)
				return conta[0];
			else return new Contas();

		}
	
		public System.Collections.Generic.List<Contas> acessarContas () {
			SQLiteConnection bd = SGBD.Connect();
			return bd.Query<Contas>(string.Format("Select * from Contas"));
		}
		public System.Collections.Generic.List<String> acessarNomeContas () 
		{
			SQLiteConnection bd = SGBD.Connect();
			System.Collections.Generic.List<Contas> contas = bd.Query<Contas>(string.Format("Select * from Contas"));
			System.Collections.Generic.List<String> nomes = new System.Collections.Generic.List<string>();
			foreach (Contas i in contas)
				nomes.Add(i.Nome);
			return nomes;

		}

		public void updateConta (Contas conta) {
			SQLiteConnection bd = SGBD.Connect();
			bd.Update(conta);
		}
		public void deleteConta (Contas conta) {
			SQLiteConnection bd = SGBD.Connect();
			bd.Delete(conta);
		}
		public int adicionarFornecedor (String nome) {
			SQLiteConnection bd = SGBD.Connect();
			Fornecedores fornecedor = new Fornecedores();
			fornecedor.Nome = nome;
			return bd.Insert(fornecedor);
		}
		public Fornecedores acessarFornecedor (int id) {
			SQLiteConnection bd = SGBD.Connect();
			System.Collections.Generic.List<Fornecedores> fornecedor = bd.Query<Fornecedores>(string.Format("Select * from Fornecedores where ID = {0};", id));
			if (fornecedor.Count > 0)
				return fornecedor[0];
			else return new Fornecedores();

		}
		public System.Collections.Generic.List<Contas> acessarFornecedores () {
			SQLiteConnection bd = SGBD.Connect();
			return bd.Query<Contas>(string.Format("Select * from Fornecedores"));
		}
		public void updateFornecedor (Fornecedores fornecedor) {
			SQLiteConnection bd = SGBD.Connect();
			bd.Update(fornecedor);
		}
		public void deleteFornecedor (Fornecedores fornecedor) {
			SQLiteConnection bd = SGBD.Connect();
			bd.Delete(fornecedor);
		}

	}
}
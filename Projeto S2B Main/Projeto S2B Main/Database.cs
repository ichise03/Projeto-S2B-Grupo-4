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
}
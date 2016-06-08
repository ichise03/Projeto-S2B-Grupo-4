using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Projeto_S2B_Main {
	[Activity(Label = "Projeto Financeiro", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity {	

		protected override void OnCreate (Bundle bundle) {
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);
            
            //CREATE DATABASE IF NOT EXISTS
            SGBD.CreateDatabase();
           
            //Comandos para tratar o click dos botões

            FindViewById(Resource.Id.contas).Click += Contas;

            FindViewById(Resource.Id.categorias).Click += Categorias;

            FindViewById(Resource.Id.lancamentos).Click += Lancamentos;

            FindViewById(Resource.Id.transferencia).Click += Transferencias;
        }

        //Funções para iniciar as novas activities depois do click

        void Contas(object sender, EventArgs e)
        {
            StartActivity(typeof(telacontas));
        }

        void Categorias(object sender, EventArgs e)
        {
            StartActivity(typeof(telacategorias));
        }

        void Lancamentos(object sender, EventArgs e)
        {
            StartActivity(typeof(telalancamentos));
        }

        void Transferencias(object sender, EventArgs e)
        {
            StartActivity(typeof(telatransferencias));
        }
	}
}


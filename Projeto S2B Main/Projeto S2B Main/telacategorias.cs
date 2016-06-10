using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Projeto_S2B_Main {
	[Activity(Label = "Categorias")]

	class telacategorias : Activity {
		private string grupo = "";
		private List<Categorias> DADOS = new List<Categorias>();

		protected override void OnCreate (Bundle bundle) {
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.telacategorias);
			
			LoadList();

			//Ativa o botão de voltar na action bar
			this.ActionBar.SetDisplayHomeAsUpEnabled(true);

			FindViewById<Button>(Resource.Id.criarCategoria).Click += NovaCategoria;
			FindViewById<ListView>(Resource.Id.categoriasView).ItemClick += List_ItemClick;
		}

		public void LoadList () {
			//Criando a listview e passando os parâmetros
			List<string> nomes = new List<string>();
			
			//Aqui serão adicionados os dados do DB
			if (grupo == "") {
				DADOS = GerenciadorBanco.SelectGruposWithTable();
				this.ActionBar.Title = "Categorias";

				DADOS.ForEach((Categorias i) => {
					nomes.Add(i.Grupo);
				});
			} else {
				DADOS = GerenciadorBanco.acessarCategorias("WHERE Grupo = \"" + grupo + "\"");

				if (DADOS.Count == 0) {
					grupo = "";
					LoadList();
					return;
				} else {
					this.ActionBar.Title = grupo + "/";
				}

				DADOS.ForEach((Categorias i) => {
					nomes.Add(i.Nome);
				});
			}

			GerenciamentoLista GL = new GerenciamentoLista(nomes, this);

			FindViewById<ListView>(Resource.Id.categoriasView).Adapter = GL;
		}

		//Função para iniciar a activity de nova categoria
		void NovaCategoria (object sender, EventArgs e) {
			StartActivity(typeof(telacriarcategoria));
		}

		//Função que define o que acontece quando clica no item da listview
		void List_ItemClick (object sender, AdapterView.ItemClickEventArgs e) {
			//Acessar grupo
			if (grupo == "") {
				grupo = DADOS[e.Position].Grupo;

				LoadList();
			} else {
				//Editar categoria
				Intent intent = new Intent(this, typeof(telacriarcategoria));

				intent.PutExtra("isUpdate", true);
				intent.PutExtra("categoriaID", DADOS[e.Position].ID);
				intent.PutExtra("categoriaNome", DADOS[e.Position].Nome);
				intent.PutExtra("categoriaGrupo", DADOS[e.Position].Grupo);

				StartActivity(intent);
			}
		}

		//Função chamada ao voltar da tela de criação de contas
		protected override void OnRestart () {
			base.OnRestart();

			//Recarregar lista
			LoadList();
		}

		//Função que faz o botão de voltar da action bar funcionar
		public override bool OnOptionsItemSelected (IMenuItem item) {
			switch (item.ItemId) {
				case Android.Resource.Id.Home:
					//Voltar para a Main
					if (grupo == "") {
						Finish();
					} else {
						//Voltar para a escolha de grupos
						grupo = "";
						LoadList();
					}

					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}

		}
	}
}
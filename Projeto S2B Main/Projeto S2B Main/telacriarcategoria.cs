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
	[Activity(Label = "Criar Categoria")]

	class telacriarcategoria : Activity {
		private int categoriaID = -1;
		private List<int> deletedIDs = new List<int>();
		private List<Atributos> DADOS = new List<Atributos>();

		protected override void OnCreate (Bundle bundle) {
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.telacriarcategoria);

			//Entrar no modo de atualização da categoria
			if (Intent.GetBooleanExtra("isUpdate", false)) {
				categoriaID = Intent.GetIntExtra("categoriaID", -1);

				FindViewById<EditText>(Resource.Id.categoriaNome).Text = Intent.GetStringExtra("categoriaNome");
				FindViewById<EditText>(Resource.Id.autoCompleteGrupo).Text = Intent.GetStringExtra("categoriaGrupo");

				FindViewById<Button>(Resource.Id.novaCategoria).Text = "ATUALIZAR CATEGORIA";

				FindViewById<Button>(Resource.Id.excluirCategoria).Enabled = true;
				FindViewById<Button>(Resource.Id.excluirCategoria).Visibility = ViewStates.Visible;

				this.ActionBar.Title = "Editar Categoria";
			} else {
				//Entrar no modo de criação da categoria
				FindViewById<Button>(Resource.Id.excluirCategoria).Enabled = false;
				FindViewById<Button>(Resource.Id.excluirCategoria).Visibility = ViewStates.Invisible;

				FindViewById<ListView>(Resource.Id.atributosView).Enabled = false;
				FindViewById<ListView>(Resource.Id.atributosView).Visibility = ViewStates.Invisible;

				FindViewById<TextView>(Resource.Id.adicionarAttr).Enabled = false;
				FindViewById<TextView>(Resource.Id.adicionarAttr).Visibility = ViewStates.Invisible;

				FindViewById<Button>(Resource.Id.criarAtributo).Enabled = false;
				FindViewById<Button>(Resource.Id.criarAtributo).Visibility = ViewStates.Invisible;

				this.ActionBar.Title = "Criar Categoria";
			}
			
			//Ativa o botão de voltar na action bar
			this.ActionBar.SetDisplayHomeAsUpEnabled(true);
			
			string[] autoCompleteOptions = GerenciadorBanco.SelectGrupos();
			ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleDropDownItem1Line, autoCompleteOptions);

			FindViewById<AutoCompleteTextView>(Resource.Id.autoCompleteGrupo).Adapter = adapter;

			LoadAtributos();
			FindViewById<ListView>(Resource.Id.atributosView).ItemClick += List_ItemClick;

			FindViewById<Button>(Resource.Id.criarAtributo).Click += NovoAtributo;
			FindViewById<Button>(Resource.Id.novaCategoria).Click += CriarCategoria;
			FindViewById<Button>(Resource.Id.excluirCategoria).Click += ExcluirCategoria;
		}

		public void LoadAtributos () {
			List<string> nomes = new List<string>();

			if (categoriaID != -1) {
				DADOS = GerenciadorBanco.acessarAtributosPorCategoria(categoriaID);
			}

			if (DADOS != null) {
				DADOS.ForEach((Atributos a) => {
					nomes.Add(a.Nome);
				});

				GerenciamentoLista GL = new GerenciamentoLista(nomes, this);
				FindViewById<ListView>(Resource.Id.atributosView).Adapter = GL;
			}
		}

		//Função que define o que acontece quando clica no item da listview
		void List_ItemClick (object sender, AdapterView.ItemClickEventArgs e) {
			//Editar atributo
			Intent intent = new Intent(this, typeof(telacriaratributo));

			intent.PutExtra("isUpdate", true);
			intent.PutExtra("atributoID", DADOS[e.Position].ID);
			intent.PutExtra("atributoNome", DADOS[e.Position].Nome);
			intent.PutExtra("atributoTipo", DADOS[e.Position].Tipo.ToString());
			intent.PutExtra("position", e.Position);

			StartActivityForResult(intent, 0);
		}

		public void NovoAtributo (object sender, EventArgs e) {
			//Abrir tela de criação de atributo
			StartActivityForResult(typeof(telacriaratributo), 0);
		}

		protected override void OnActivityResult (int requestCode, [GeneratedEnum] Result resultCode, Intent data) {
			base.OnActivityResult(requestCode, resultCode, data);

			bool isUpdate = data.GetBooleanExtra("isUpdate", false);

			if (resultCode == Result.Ok) {
				List<string> nomes = new List<string>();

				if (data.GetBooleanExtra("Delete", false)) {
					deletedIDs.Add(DADOS[data.GetIntExtra("Position", 0)].ID);
					DADOS.RemoveAt(data.GetIntExtra("Position", 0));
					Toast.MakeText(this, "Atributo removido.", ToastLength.Short).Show();
				} else {
					TipoAtributo tipo;

					switch (data.GetStringExtra("AtributoTipo")) {
						case "Booleano":
							tipo = TipoAtributo.Booleano;
							break;
						case "Número":
							tipo = TipoAtributo.Numero;
							break;
						case "Texto":
						default:
							tipo = TipoAtributo.Texto;
							break;
					}

					if (isUpdate) {
						DADOS[data.GetIntExtra("Position", 0)] = new Atributos(categoriaID, data.GetStringExtra("AtributoNome"), tipo);
						Toast.MakeText(this, "Atributo atualizado.", ToastLength.Short).Show();
					} else {
						DADOS.Add(new Atributos(categoriaID, data.GetStringExtra("AtributoNome"), tipo));
						Toast.MakeText(this, "Atributo adicionado.", ToastLength.Short).Show();
					}
				}

				DADOS.ForEach((Atributos a) => {
					nomes.Add(a.Nome);
				});

				GerenciamentoLista GL = new GerenciamentoLista(nomes, this);
				FindViewById<ListView>(Resource.Id.atributosView).Adapter = GL;
			} else {
				if (isUpdate)
					Toast.MakeText(this, "Erro ao editar o atributo.", ToastLength.Short).Show();
				else
					Toast.MakeText(this, "Erro ao criar o atributo.", ToastLength.Short).Show();
			}
		}

		public void CriarCategoria (object sender, EventArgs e) {
			try {
				if (categoriaID == -1) {
					categoriaID = GerenciadorBanco.adicionarCategorias(FindViewById<EditText>(Resource.Id.categoriaNome).Text, FindViewById<EditText>(Resource.Id.autoCompleteGrupo).Text);

					Toast.MakeText(this, "Categoria criada com sucesso.", ToastLength.Short).Show();
					Finish();
				} else {
					Categorias categoriaAtualizada = new Categorias(FindViewById<EditText>(Resource.Id.categoriaNome).Text, FindViewById<EditText>(Resource.Id.autoCompleteGrupo).Text, categoriaID);
					GerenciadorBanco.updateCategoria(categoriaAtualizada);

					try {
						foreach (Atributos a in DADOS) {
							if (a.ID >= 0) {
								a.ID_Categoria = categoriaID;
								GerenciadorBanco.updateAtributo(a);
							} else {
								GerenciadorBanco.adicionarAtributo(categoriaID, a.Nome, a.Tipo);
							}
						}

						foreach (int id in deletedIDs) {
							if (id >= 0) {
								GerenciadorBanco.deleteAtributos(id);
							}
						}

						Toast.MakeText(this, "Categoria atualizada com sucesso.", ToastLength.Short).Show();
						Finish();
					} catch {
						Toast.MakeText(this, "Erro ao criar o(s) atributo(s).", ToastLength.Short).Show();
					}
				}
			} catch {
				if (categoriaID == -1)
					Toast.MakeText(this, "Erro ao criar nova categoria.", ToastLength.Short).Show();
				else
					Toast.MakeText(this, "Erro ao atualizar a categoria.", ToastLength.Short).Show();
			}
		}

		void ExcluirCategoria (object sender, EventArgs e) {
			try {
				GerenciadorBanco.deleteAtributosPelaCategoria(categoriaID);
				GerenciadorBanco.deleteCategoria(categoriaID);

				Toast.MakeText(this, "Categoria excluida com sucesso.", ToastLength.Short).Show();
				Finish();
			} catch {
				Toast.MakeText(this, "Erro ao excluir a categoria.", ToastLength.Short).Show();
			}
		}

		//Função que faz o botão de voltar da action bar funcionar
		public override bool OnOptionsItemSelected (IMenuItem item) {
			switch (item.ItemId) {
				case Android.Resource.Id.Home:
					Finish();
					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}
		}
	}
}
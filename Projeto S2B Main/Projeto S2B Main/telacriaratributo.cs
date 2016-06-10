using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Projeto_S2B_Main {
	[Activity(Label = "Atributos")]

	class telacriaratributo : Activity {
		private int atributoID = -1;
		private int position = -1;
		private bool isUpdate = false;

		protected override void OnCreate (Bundle bundle) {
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.telacriaratributo);

			ArrayAdapter ad = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, new string[] { "Booleano", "Número", "Texto" });
			FindViewById<Spinner>(Resource.Id.atributoTipo).Adapter = ad;

			isUpdate = Intent.GetBooleanExtra("isUpdate", false);

			//Entrar no modo de atualização do atributo
			if (isUpdate) {
				atributoID = Intent.GetIntExtra("categoriaID", -1);
				position = Intent.GetIntExtra("position", -1);

				FindViewById<EditText>(Resource.Id.atributoNome).Text = Intent.GetStringExtra("atributoNome");
				
				switch (Intent.GetStringExtra("atributoTipo")) {
					case "Booleano":
						FindViewById<Spinner>(Resource.Id.atributoTipo).SetSelection(0);
						break;
					case "Numero":
						FindViewById<Spinner>(Resource.Id.atributoTipo).SetSelection(1);
						break;
					case "Texto":
						FindViewById<Spinner>(Resource.Id.atributoTipo).SetSelection(2);
						break;
				}

				FindViewById<Button>(Resource.Id.excluirAtributo).Enabled = true;
				FindViewById<Button>(Resource.Id.excluirAtributo).Visibility = ViewStates.Visible;

				this.ActionBar.Title = "Editar Atributo";
			} else {
				//Entrar no modo de criação do atributo
				FindViewById<Button>(Resource.Id.excluirAtributo).Enabled = false;
				FindViewById<Button>(Resource.Id.excluirAtributo).Visibility = ViewStates.Invisible;

				this.ActionBar.Title = "Adicionar Atributo";
			}

			//Ativa o botão de voltar na action bar
			this.ActionBar.SetDisplayHomeAsUpEnabled(true);

			FindViewById<Button>(Resource.Id.excluirAtributo).Click += DeleteAtributo;
			FindViewById<Button>(Resource.Id.adicionarAtributo).Click += CriarAtributo;
		}

		public void DeleteAtributo (object sender, EventArgs e) {
			Intent result = new Intent(this, typeof(telacategorias));
			result.PutExtra("isUpdate", isUpdate);
			result.PutExtra("Delete", true);
			result.PutExtra("Position", position);

			SetResult(Result.Ok, result);
			Finish();
		}

		public void CriarAtributo (object sender, EventArgs e) {
			Intent result = new Intent(this, typeof(telacategorias));
			result.PutExtra("isUpdate", isUpdate);
			result.PutExtra("Delete", false);
			result.PutExtra("Position", position);
			result.PutExtra("AtributoNome", FindViewById<TextView>(Resource.Id.atributoNome).Text);
			result.PutExtra("AtributoTipo", FindViewById<Spinner>(Resource.Id.atributoTipo).SelectedItemPosition);

			SetResult(Result.Ok, result);
			Finish();
		}

		//Função que faz o botão de voltar da action bar funcionar
		public override bool OnOptionsItemSelected (IMenuItem item) {
			switch (item.ItemId) {
				case Android.Resource.Id.Home:
					SetResult(Result.Canceled);
					Finish();
					return true;
				default:
					return base.OnOptionsItemSelected(item);
			}

		}
	}
}
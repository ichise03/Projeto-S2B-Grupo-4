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
	[Activity(Label = "Nova Conta")]

	class telacriarconta : Activity {
		private int contaID = -1;

		protected override void OnCreate (Bundle bundle) {
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.telacriarconta);

			//Entrar no modo de atualiza��o da conta
			if (Intent.GetBooleanExtra("isUpdate", false)) {
				contaID = Intent.GetIntExtra("contaID", -1);

				FindViewById<EditText>(Resource.Id.editText1).Text = Intent.GetStringExtra("contaNome");
				FindViewById<EditText>(Resource.Id.editText2).Text = Intent.GetStringExtra("contaSaldo");
				switch (Intent.GetStringExtra("contaTipo")) {
					case "Moeda_Em_Esp�cie":
						FindViewById<RadioButton>(Resource.Id.radioButton1).Checked = true;
						break;
					case "Cart�o_De_Cr�dito":
						FindViewById<RadioButton>(Resource.Id.radioButton2).Checked = true;
						break;
					case "Cart�o_De_D�bito":
						FindViewById<RadioButton>(Resource.Id.radioButton3).Checked = true;
						break;
					case "Poupan�a":
					default:
						FindViewById<RadioButton>(Resource.Id.radioButton4).Checked = true;
						break;
				}

				FindViewById<TextView>(Resource.Id.saldoLabel).Text = "Saldo Atual";
				FindViewById<Button>(Resource.Id.contaButton).Text = "ATUALIZAR CONTA";

				FindViewById<Button>(Resource.Id.excluirConta).Enabled = true;
				FindViewById<Button>(Resource.Id.excluirConta).Visibility = ViewStates.Visible;
			} else {
				//Entrar no modo de cria��o da conta
				FindViewById<TextView>(Resource.Id.saldoLabel).Text = "Saldo Inicial";
				FindViewById<Button>(Resource.Id.contaButton).Text = "CRIAR NOVA CONTA";

				FindViewById<Button>(Resource.Id.excluirConta).Enabled = false;
				FindViewById<Button>(Resource.Id.excluirConta).Visibility = ViewStates.Invisible;
			}

			//Ativa o bot�o de voltar na action bar
			this.ActionBar.SetDisplayHomeAsUpEnabled(true);

			FindViewById(Resource.Id.contaButton).Click += CriarConta;
			FindViewById(Resource.Id.excluirConta).Click += ExcluirConta;
		}

		//Cria ou atualiza a conta
		void CriarConta (object sender, EventArgs e) {
			TipoConta tipo = new TipoConta();

			if (FindViewById<RadioButton>(Resource.Id.radioButton1).Checked) {
				tipo = TipoConta.Moeda_Em_Esp�cie;
			} else if (FindViewById<RadioButton>(Resource.Id.radioButton2).Checked) {
				tipo = TipoConta.Cart�o_De_Cr�dito;
			} else if (FindViewById<RadioButton>(Resource.Id.radioButton3).Checked) {
				tipo = TipoConta.Cart�o_De_D�bito;
			} else if (FindViewById<RadioButton>(Resource.Id.radioButton4).Checked) {
				tipo = TipoConta.Poupan�a;
			}

			decimal saldo;
			if (!decimal.TryParse(FindViewById<EditText>(Resource.Id.editText2).Text, out saldo))
				saldo = 0;

			try {
				if (contaID == -1) {
					GerenciadorBanco.adicionarConta(FindViewById<EditText>(Resource.Id.editText1).Text, saldo, tipo);

					Toast.MakeText(this, "Conta criada com sucesso.", ToastLength.Short).Show();
				} else {
					Contas contaAtualizada = new Contas(saldo, tipo, FindViewById<EditText>(Resource.Id.editText1).Text, contaID);
					GerenciadorBanco.updateConta(contaAtualizada);

					Toast.MakeText(this, "Conta atualizada com sucesso.", ToastLength.Short).Show();
				}

				Finish();
			} catch {
				if (contaID == -1)
					Toast.MakeText(this, "Erro ao criar nova conta.", ToastLength.Short).Show();
				else
					Toast.MakeText(this, "Erro ao atualizar a conta.", ToastLength.Short).Show();
			}
		}

		void ExcluirConta (object sender, EventArgs e) {
			try {
				GerenciadorBanco.deleteConta(contaID);

				Toast.MakeText(this, "Conta excluida com sucesso.", ToastLength.Short).Show();
				Finish();
			} catch {
				Toast.MakeText(this, "Erro ao excluir a conta.", ToastLength.Short).Show();
			}
		}

		//Fun��o que faz o bot�o de voltar da action bar funcionar
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
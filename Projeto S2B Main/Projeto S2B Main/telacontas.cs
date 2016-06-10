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
	[Activity(Label = "Contas")]

	class telacontas : Activity {
		List<Contas> DADOS = new List<Contas>();

		protected override void OnCreate (Bundle bundle) {
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.telacontas);

			this.ActionBar.SetDisplayHomeAsUpEnabled(true);

			LoadList();

			FindViewById<ListView>(Resource.Id.contasView).ItemClick += List_ItemClick;

			FindViewById(Resource.Id.criarConta).Click += NovaConta;
		}

		public void LoadList () {
			//Aqui ser�o adicionados os dados do DB
			DADOS = GerenciadorBanco.acessarContas();

			GerenciamentoContas GL = new GerenciamentoContas(DADOS, this);

			FindViewById<ListView>(Resource.Id.contasView).Adapter = GL;
		}

		//Fun��o para iniciar a activity de nova conta
		void NovaConta (object sender, EventArgs e) {
			StartActivity(typeof(telacriarconta));
		}

		//Fun��o que define o que acontece quando clica no item da listview
		void List_ItemClick (object sender, AdapterView.ItemClickEventArgs e) {
			Intent intent = new Intent(this, typeof(telacriarconta));

			intent.PutExtra("isUpdate", true);
			intent.PutExtra("contaID", DADOS[e.Position].ID);
			intent.PutExtra("contaNome", DADOS[e.Position].Nome);
			intent.PutExtra("contaSaldo", GerenciadorBanco.Moeda(DADOS[e.Position].Saldo));
			intent.PutExtra("contaTipo", DADOS[e.Position].Tipo.ToString());

			StartActivity(intent);
		}

		//Fun��o chamada ao voltar da tela de cria��o de contas
		protected override void OnRestart () {
			base.OnRestart();

			//Recarregar lista
			LoadList();
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
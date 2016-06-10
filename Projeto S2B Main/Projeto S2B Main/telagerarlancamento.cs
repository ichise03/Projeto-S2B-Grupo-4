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

namespace Projeto_S2B_Main
{
    [Activity(Label = "Gerar Lançamento")]

    class telagerarlancamento : Activity
    {
		private List<Contas> list;
		private List<Categorias> list2;

		protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.telagerarlancamento);

            //Ativa o botão de voltar na action bar
            this.ActionBar.SetDisplayHomeAsUpEnabled(true);

			List<string> nomes = new List<string>();
			list = GerenciadorBanco.acessarContas();

			list.ForEach((Contas c) => {
				nomes.Add(c.Nome);
			});

			ArrayAdapter ad = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, nomes);
			FindViewById<Spinner>(Resource.Id.spinner1).Adapter = ad;

			List<string> nomes2 = new List<string>();
			list2 = GerenciadorBanco.acessarCategorias();

			list2.ForEach((Categorias c) => {
				nomes2.Add(c.Nome);
			});

			ArrayAdapter ad2 = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, nomes2);
			FindViewById<Spinner>(Resource.Id.spinner3).Adapter = ad2;

			FindViewById<Button>(Resource.Id.button1).Click += Lancar;
		}

		public void Lancar (object sender, EventArgs e) {
			TipoLancamento tipo;

			if (FindViewById<RadioButton>(Resource.Id.radioButton1).Checked) {
				tipo = TipoLancamento.Creditar;
			} else {
				tipo = TipoLancamento.Debitar;
			}

			int contaID = list[FindViewById<Spinner>(Resource.Id.spinner1).SelectedItemPosition].ID;
			int categoriaID = list2[FindViewById<Spinner>(Resource.Id.spinner3).SelectedItemPosition].ID;

			GerenciadorBanco.adicionarLancamento(contaID, 1, categoriaID, decimal.Parse(FindViewById<EditText>(Resource.Id.editText1).Text.Replace(",", ".")), tipo, Convert.ToDateTime(FindViewById<EditText>(Resource.Id.editText5).Text + " " + FindViewById<EditText>(Resource.Id.editText4).Text), FindViewById<EditText>(Resource.Id.editText2).Text);
		}

        //Função que faz o botão de voltar da action bar funcionar
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }

        }
    }
}
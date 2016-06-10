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
	class GerenciamentoContas : BaseAdapter<string> {
		List<Contas> DADOS;
		Activity C;

		public GerenciamentoContas (List<Contas> dados, Activity c) {
			DADOS = dados;
			C = c;
		}

		public override int Count {
			get {
				return DADOS.Count;
			}
		}

		public override string this[int position] {
			get {
				return DADOS[position].Nome;
			}
		}

		public override long GetItemId (int position) {
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent) {
			View view = convertView;
			if (view == null)
				view = C.LayoutInflater.Inflate(Resource.Layout.celulaConta, null);

			view.FindViewById<TextView>(Resource.Id.textLabel).Text = DADOS[position].Nome;
			view.FindViewById<TextView>(Resource.Id.moneyLabel).Text = "Saldo: R$" + GerenciadorBanco.Moeda(DADOS[position].Saldo);

			if (DADOS[position].Saldo < 0)
				view.FindViewById<TextView>(Resource.Id.moneyLabel).SetTextColor(Android.Graphics.Color.Red);
			else
				view.FindViewById<TextView>(Resource.Id.moneyLabel).SetTextColor(Android.Graphics.Color.LimeGreen);

			switch (DADOS[position].Tipo) {
				case TipoConta.Cartão_De_Crédito:
					view.FindViewById<ImageView>(Resource.Id.typeAccLabel).SetImageResource(Resource.Drawable.creditCard);
					break;
				case TipoConta.Cartão_De_Débito:
					view.FindViewById<ImageView>(Resource.Id.typeAccLabel).SetImageResource(Resource.Drawable.debitCard);
					break;
				case TipoConta.Moeda_Em_Espécie:
					view.FindViewById<ImageView>(Resource.Id.typeAccLabel).SetImageResource(Resource.Drawable.money);
					break;
				case TipoConta.Poupança:
					view.FindViewById<ImageView>(Resource.Id.typeAccLabel).SetImageResource(Resource.Drawable.poupanca);
					break;

			}

			return view;
		}
	}
}
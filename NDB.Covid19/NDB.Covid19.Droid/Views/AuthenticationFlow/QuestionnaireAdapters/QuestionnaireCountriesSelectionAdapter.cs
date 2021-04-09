using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Java.Lang;
using NDB.Covid19.ViewModels;

namespace NDB.Covid19.Droid.Views.AuthenticationFlow.QuestionnaireAdapters
{
    internal class QuestionnaireCountriesSelectionAdapter : RecyclerView.Adapter
    {
        private readonly List<int> _selectedItems = new List<int>();
        private List<CountryDetailsViewModel> _countryList;

        public QuestionnaireCountriesSelectionAdapter(List<CountryDetailsViewModel> countryList)
        {
            _countryList = countryList;
        }

        private List<CountryDetailsViewModel> Data
        {
            get => _countryList;
            set
            {
                _countryList = value;
                NotifyDataSetChanged();
            }
        }

        public override int ItemCount => Data.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            CountryDetailsViewModel item = Data[position];
            if (holder is QuestionnaireCountriesSelectionAdapterViewHolder viewHolder)
            {
                viewHolder.Caption.Text = item.Name;
                viewHolder.Check.Checked = false || _selectedItems.Contains(position);

                viewHolder.SetOnClickListener(new CheckedChangeListener(this, holder, position));
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = LayoutInflater
                .From(parent.Context)
                .Inflate(Resource.Layout.country_view, parent, false) as LinearLayout;
            return new QuestionnaireCountriesSelectionAdapterViewHolder(view);
        }

        private class CheckedChangeListener : Object, View.IOnClickListener
        {
            private readonly RecyclerView.ViewHolder _holder;
            private readonly int _position;
            private readonly QuestionnaireCountriesSelectionAdapter _self;

            public CheckedChangeListener(QuestionnaireCountriesSelectionAdapter self, RecyclerView.ViewHolder holder,
                int position)
            {
                _holder = holder;
                _self = self;
                _position = position;
            }

            public void OnClick(View v)
            {
                var vh = (QuestionnaireCountriesSelectionAdapterViewHolder) _holder;
                vh.Check.Checked = !vh.Check.Checked;
                _self.Data[_position].Checked = vh.Check.Checked;
            }
        }
    }

    internal class QuestionnaireCountriesSelectionAdapterViewHolder : RecyclerView.ViewHolder
    {
        private readonly View _itemView;

        public QuestionnaireCountriesSelectionAdapterViewHolder(View item) : base(item)
        {
            _itemView = item;

            Check = item.FindViewById<CheckBox>(Resource.Id.country_item_checkbox);
            Caption = item.FindViewById<TextView>(Resource.Id.country_item_caption);

            Check.Clickable = false;
        }

        public CheckBox Check { get; set; }
        public TextView Caption { get; set; }

        public void SetOnClickListener(View.IOnClickListener onClickListener)
        {
            _itemView.SetOnClickListener(onClickListener);
        }
    }
}
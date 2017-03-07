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
using Android.Text;
using Java.Lang;

namespace JoyaTouchCradleSampleAPI
{
    public class ConfigAreaAdapter : BaseAdapter
    {

        Context context;

        private byte[] values;

        public ConfigAreaAdapter(Context context, byte[] values)
        {
            this.context = context;
            this.values = values;
        }

        public override int Count
        {
            get { return values.Length; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return values[position];
        }

        public void SetItem(int position, byte val)
        {
            values[position] = val;
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder = null;

            if (convertView == null)
            {
                LayoutInflater inflater = LayoutInflater.From(context);
                convertView = inflater.Inflate(Resource.Layout.config_area_grid_cell, parent, false);
                holder = new ViewHolder();
                holder.editText = (EditText)convertView.FindViewById(Resource.Id.editTextConfigAreaGridCell);
                convertView.Tag = holder;
            }
            else
            {
                holder = (ViewHolder)convertView.Tag;
            }

            if (holder.textWatcher != null)
                holder.editText.RemoveTextChangedListener(holder.textWatcher);

            holder.editText.Text = "" + (values[position] & 0xFF);
            holder.textWatcher = new TextWatcherCell(position, this);
            holder.editText.AddTextChangedListener(holder.textWatcher);
            return convertView;
        }

    }

    public class ViewHolder : Java.Lang.Object
    {
        public EditText editText;
        public TextWatcherCell textWatcher;
    }

    public class TextWatcherCell : Java.Lang.Object, ITextWatcher
    {

        private int position;
        private ConfigAreaAdapter adapter;

        public TextWatcherCell(int position, ConfigAreaAdapter adapter)
        {
            this.position = position;
            this.adapter = adapter;
        }

        public void AfterTextChanged(IEditable e)
        {
            try
            {
                int tmp = Integer.ParseInt(e.ToString());
                //adapter.NotifyDataSetChanged();
                adapter.SetItem(position, (byte)tmp);
                // Java code was somehow able to access values[] from parent directly??
                //values[position] = (byte)tmp;
            }
            catch (NumberFormatException)
            {

            }
        }

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {

        }

        public void OnTextChanged(ICharSequence s, int start, int before, int count)
        {

        }
    }
}
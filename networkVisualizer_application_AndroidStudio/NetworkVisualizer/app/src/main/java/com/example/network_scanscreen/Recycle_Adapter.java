package com.example.network_scanscreen;

import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import org.w3c.dom.Text;

import java.util.ArrayList;

public class Recycle_Adapter extends RecyclerView.Adapter<Recycle_Adapter.mViewHolder> {
    private ArrayList<Card_java> card_javas;

    public static class mViewHolder extends RecyclerView.ViewHolder {
        public ImageView mImageView;
        public TextView mTextView1;
        public TextView mTextView2;
        public TextView mTextView3;
        public TextView mTextView4;

        public mViewHolder(@NonNull View itemView) {
            super(itemView);
            mImageView = itemView.findViewById(R.id.imageView);
            mTextView1 = itemView.findViewById(R.id.text1);
            mTextView2 = itemView.findViewById(R.id.text2);
            mTextView3 = itemView.findViewById(R.id.text3);
            mTextView4 = itemView.findViewById(R.id.text4);
        }
    }

    public Recycle_Adapter(ArrayList<Card_java> cardList) {
        card_javas = cardList;
    }

    @NonNull
    @Override
    public mViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View V = LayoutInflater.from(parent.getContext()).inflate(R.layout.card_view, parent, false);
        mViewHolder vh = new mViewHolder(V);
        return vh;

    }

    @Override
    public void onBindViewHolder(@NonNull mViewHolder holder, int position) {
        Card_java currentCard = card_javas.get(position);

        holder.mImageView.setImageResource(currentCard.getImageResource());
        holder.mTextView1.setText(currentCard.getText1());
        holder.mTextView2.setText(currentCard.getText2());
        holder.mTextView3.setText(currentCard.getText3());
        holder.mTextView4.setText(currentCard.getText4());
    }
    
    @Override
    public int getItemCount() {
        return card_javas.size();
    }

    public void filterList(ArrayList<Card_java> filteredList) {
        card_javas = filteredList;
        notifyDataSetChanged();
    }
}

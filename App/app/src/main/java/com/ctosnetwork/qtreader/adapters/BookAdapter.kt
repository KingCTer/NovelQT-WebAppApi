/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 14:00, 22/10/2021
 */

package com.ctosnetwork.qtreader.adapters

import android.util.Log
import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.paging.PagingDataAdapter
import androidx.recyclerview.widget.DiffUtil
import androidx.recyclerview.widget.RecyclerView
import com.bumptech.glide.Glide
import com.bumptech.glide.load.engine.DiskCacheStrategy
import com.ctosnetwork.qtreader.R
import com.ctosnetwork.qtreader.api.data.DataBook
import com.ctosnetwork.qtreader.databinding.ItemBookVerticalCardBinding

class BookAdapter(private val viewType: Int) : PagingDataAdapter<DataBook, RecyclerView.ViewHolder>(DataBookDiffCallback()) {

    companion object {
        const val VIEW_TYPE_VERTICAL_CARD: Int = 0
    }

    override fun getItemViewType(position: Int): Int {
        return viewType
    }

    override fun onBindViewHolder(holder: RecyclerView.ViewHolder, position: Int){
        val book = getItem(position)

        if(book != null) {
            when(holder.itemViewType) {
                Companion.VIEW_TYPE_VERTICAL_CARD -> (holder as BookVerticalViewHolder).bind(book)
                else -> (holder as BookVerticalViewHolder).bind(book)
            }
        }

    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): RecyclerView.ViewHolder {
        when(viewType) {
            Companion.VIEW_TYPE_VERTICAL_CARD -> {
                return BookVerticalViewHolder(
                    ItemBookVerticalCardBinding.inflate(LayoutInflater.from(parent.context), parent, false)
                )
            }
            else -> return BookVerticalViewHolder(ItemBookVerticalCardBinding.inflate(LayoutInflater.from(parent.context), parent, false))
        }
    }

    class BookVerticalViewHolder(
        private val binding: ItemBookVerticalCardBinding
    ) : RecyclerView.ViewHolder(binding.root) {
        init {
            binding.setClickListener { view ->
                binding.book?.let { book ->
                    Log.e("mTest", book.id)
                }
            }
        }

        fun bind(item: DataBook) {
            binding.apply {
                book = item
                executePendingBindings()
            }

            Glide.with(binding.root)
                .load(item.cover)
                .diskCacheStrategy(DiskCacheStrategy.ALL)
                .placeholder(R.drawable.qd_book_cover)
                .into(binding.bookImage)
        }
    }



}

private class DataBookDiffCallback : DiffUtil.ItemCallback<DataBook>() {
    override fun areItemsTheSame(oldItem: DataBook, newItem: DataBook): Boolean {
        return oldItem.id == newItem.id
    }

    override fun areContentsTheSame(oldItem: DataBook, newItem: DataBook): Boolean {
        return oldItem == newItem
    }
}
/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 14:00, 22/10/2021
 */

package com.ctosnetwork.qtreader.adapters

import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.core.view.marginEnd
import androidx.navigation.findNavController
import androidx.paging.PagingDataAdapter
import androidx.recyclerview.widget.DiffUtil
import androidx.recyclerview.widget.RecyclerView
import com.bumptech.glide.Glide
import com.bumptech.glide.load.engine.DiskCacheStrategy
import com.ctosnetwork.qtreader.R
import com.ctosnetwork.qtreader.api.data.DataBook
import com.ctosnetwork.qtreader.databinding.ItemBookHorizontalCardBinding
import com.ctosnetwork.qtreader.databinding.ItemBookHorizontalCardListBinding
import com.ctosnetwork.qtreader.databinding.ItemBookVerticalCardBinding
import com.ctosnetwork.qtreader.ui.book.BookListFragmentDirections
import com.ctosnetwork.qtreader.ui.library.LibraryFragmentDirections

class BookAdapter(private val viewType: Int) :
    PagingDataAdapter<DataBook, RecyclerView.ViewHolder>(DataBookDiffCallback()) {

    companion object {
        const val VIEW_TYPE_VERTICAL_CARD: Int = 0
        const val VIEW_TYPE_HORIZONTAL_CARD: Int = 1
        const val VIEW_TYPE_HORIZONTAL_CARD_LIST: Int = 2
    }

    override fun getItemViewType(position: Int): Int {
        return viewType
    }

    override fun onBindViewHolder(holder: RecyclerView.ViewHolder, position: Int) {
        val book = getItem(position)

        if (book != null) {
            when (holder.itemViewType) {
                Companion.VIEW_TYPE_VERTICAL_CARD -> (holder as BookVerticalViewHolder).bind(book)
                Companion.VIEW_TYPE_HORIZONTAL_CARD -> (holder as BookHorizontalViewHolder).bind(
                    book
                )
                Companion.VIEW_TYPE_HORIZONTAL_CARD_LIST -> (holder as BookHorizontalListViewHolder).bind(
                    book
                )
                else -> (holder as BookVerticalViewHolder).bind(book)
            }
        }

    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): RecyclerView.ViewHolder {
        return when (viewType) {
            Companion.VIEW_TYPE_VERTICAL_CARD -> {
                BookVerticalViewHolder(
                    ItemBookVerticalCardBinding.inflate(
                        LayoutInflater.from(parent.context),
                        parent,
                        false
                    )
                )
            }
            Companion.VIEW_TYPE_HORIZONTAL_CARD -> {
                BookHorizontalViewHolder(
                    ItemBookHorizontalCardBinding.inflate(
                        LayoutInflater.from(parent.context),
                        parent,
                        false
                    )
                )
            }
            Companion.VIEW_TYPE_HORIZONTAL_CARD_LIST -> {
                BookHorizontalListViewHolder(
                    ItemBookHorizontalCardListBinding.inflate(
                        LayoutInflater.from(parent.context),
                        parent,
                        false
                    )
                )
            }
            else -> BookVerticalViewHolder(
                ItemBookVerticalCardBinding.inflate(
                    LayoutInflater.from(
                        parent.context
                    ), parent, false
                )
            )
        }
    }

    class BookVerticalViewHolder(
        private val binding: ItemBookVerticalCardBinding
    ) : RecyclerView.ViewHolder(binding.root) {
        init {
            binding.setClickListener { view ->
                binding.book?.let { book ->
                    navigateToBookDetail(book, view)
                }
            }
        }

        private fun navigateToBookDetail(book: DataBook, view: View) {
            val direction =
                LibraryFragmentDirections.actionNavigationLibraryToBookDetailFragment(book.id)
            view.findNavController().navigate(direction)
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

    class BookHorizontalViewHolder(
        private val binding: ItemBookHorizontalCardBinding
    ) : RecyclerView.ViewHolder(binding.root) {
        init {
            binding.setClickListener { view ->
                binding.book?.let { book ->
                    navigateToBookDetail(book, view)
                }
            }
        }

        private fun navigateToBookDetail(book: DataBook, view: View) {
            val direction =
                LibraryFragmentDirections.actionNavigationLibraryToBookDetailFragment(book.id)
            view.findNavController().navigate(direction)
        }

        fun bind(item: DataBook) {
            binding.apply {
                book = item
                executePendingBindings()

                val chapterTotalText = item.chapterTotal.toString() + " chương"
                binding.textViewChapterTotal.text = chapterTotalText
            }

            Glide.with(binding.root)
                .load(item.cover)
                .diskCacheStrategy(DiskCacheStrategy.ALL)
                .placeholder(R.drawable.qd_book_cover)
                .into(binding.bookImage)
        }
    }

    class BookHorizontalListViewHolder(
        private val binding: ItemBookHorizontalCardListBinding
    ) : RecyclerView.ViewHolder(binding.root) {
        init {
            binding.setClickListener { view ->
                binding.book?.let { book ->
                    navigateToBookDetail(book, view)
                }
            }
        }

        private fun navigateToBookDetail(book: DataBook, view: View) {
            val direction =
                BookListFragmentDirections.actionBookListFragmentToBookDetailFragment(book.id)
            view.findNavController().navigate(direction)
        }

        fun bind(item: DataBook) {
            binding.apply {
                book = item
                executePendingBindings()

                val chapterTotalText = item.chapterTotal.toString() + " chương"
                binding.textViewListChapterTotal.text = chapterTotalText
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
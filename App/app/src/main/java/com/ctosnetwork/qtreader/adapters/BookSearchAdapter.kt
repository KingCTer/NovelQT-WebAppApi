/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 18:10, 29/10/2021
 */

package com.ctosnetwork.qtreader.adapters

import android.text.Html
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.navigation.findNavController
import androidx.paging.PagingDataAdapter
import androidx.recyclerview.widget.DiffUtil
import androidx.recyclerview.widget.RecyclerView
import com.bumptech.glide.Glide
import com.bumptech.glide.load.engine.DiskCacheStrategy
import com.ctosnetwork.qtreader.R
import com.ctosnetwork.qtreader.api.data.DataBookSearch
import com.ctosnetwork.qtreader.databinding.ItemBookSearchHorizontalCardListBinding
import com.ctosnetwork.qtreader.ui.book.BookSearchFragmentDirections

class BookSearchAdapter(
    private val viewType: Int
) : PagingDataAdapter<DataBookSearch, RecyclerView.ViewHolder>(DataBookSearchDiffCallback()) {

    companion object {
        const val VIEW_TYPE_HORIZONTAL_CARD_LIST: Int = 0
    }

    override fun getItemViewType(position: Int): Int {
        return viewType
    }

    override fun onBindViewHolder(holder: RecyclerView.ViewHolder, position: Int) {
        val item = getItem(position)

        if (item != null) {
            when (holder.itemViewType) {
                VIEW_TYPE_HORIZONTAL_CARD_LIST -> (holder as BookSearchHorizontalListViewHolder).bind(item)
                else -> (holder as BookSearchHorizontalListViewHolder).bind(item)
            }
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): RecyclerView.ViewHolder {
        return when (viewType) {
            VIEW_TYPE_HORIZONTAL_CARD_LIST -> {
                BookSearchHorizontalListViewHolder(
                    ItemBookSearchHorizontalCardListBinding.inflate(
                        LayoutInflater.from(parent.context),
                        parent,
                        false
                    )
                )
            }
            else -> BookSearchHorizontalListViewHolder(
                ItemBookSearchHorizontalCardListBinding.inflate(
                    LayoutInflater.from(parent.context),
                    parent,
                    false
                )
            )
        }
    }

    class BookSearchHorizontalListViewHolder(
        private val binding: ItemBookSearchHorizontalCardListBinding
    ) : RecyclerView.ViewHolder(binding.root) {
        init {
            binding.setClickListener { view ->
                binding.bookSearch?.let { bookSearch ->
                    navigateToBookDetail(bookSearch, view)
                }
            }
        }

        private fun navigateToBookDetail(bookSearch: DataBookSearch, view: View) {
            val direction =
                BookSearchFragmentDirections.actionBookSearchFragmentToBookDetailFragment(bookSearch.bookResult.id)
            view.findNavController().navigate(direction)
        }

        fun bind(item: DataBookSearch) {
            binding.apply {
                bookSearch = item
                executePendingBindings()

                val bookName = item.bookNameHighlight.toString()
                val bookNameSub = bookName.substring(1, bookName.length - 1)
                itemBookSearchName.text = Html.fromHtml(bookNameSub, Html.FROM_HTML_MODE_LEGACY)

                val authorName = item.authorNameHighlight.toString()
                val authorNameSub = authorName.substring(1, authorName.length - 1)
                itemBookSearchAuthor.text = Html.fromHtml(authorNameSub, Html.FROM_HTML_MODE_LEGACY)

                val chapterTotalText = item.bookResult.chapterTotal.toString() + " chương"
                binding.itemBookSearchChapter.text = chapterTotalText

                Glide.with(root)
                    .load(item.bookResult.cover)
                    .diskCacheStrategy(DiskCacheStrategy.ALL)
                    .placeholder(R.drawable.qd_book_cover)
                    .into(binding.itemBookSearchCover)
            }


        }
    }


}

private class DataBookSearchDiffCallback : DiffUtil.ItemCallback<DataBookSearch>() {
    override fun areItemsTheSame(oldItem: DataBookSearch, newItem: DataBookSearch): Boolean {
        return oldItem.bookResult.id == newItem.bookResult.id
    }

    override fun areContentsTheSame(oldItem: DataBookSearch, newItem: DataBookSearch): Boolean {
        return oldItem == newItem
    }
}
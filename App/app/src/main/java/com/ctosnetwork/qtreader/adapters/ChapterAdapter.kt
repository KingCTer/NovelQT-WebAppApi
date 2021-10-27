/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 15:28, 27/10/2021
 */

package com.ctosnetwork.qtreader.adapters

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.navigation.findNavController
import androidx.paging.PagingDataAdapter
import androidx.recyclerview.widget.DiffUtil
import androidx.recyclerview.widget.RecyclerView
import com.ctosnetwork.qtreader.api.data.DataChapter
import com.ctosnetwork.qtreader.databinding.ItemChapterHorizontalCardListBinding
import com.ctosnetwork.qtreader.ui.chapter.ChapterListFragmentDirections

class ChapterAdapter(private val viewType: Int) :
    PagingDataAdapter<DataChapter, RecyclerView.ViewHolder>(DataChapterDiffCallback()) {

    companion object {
        const val VIEW_TYPE_HORIZONTAL_CARD_LIST: Int = 0
    }

    override fun getItemViewType(position: Int): Int {
        return viewType
    }

    override fun onBindViewHolder(holder: RecyclerView.ViewHolder, position: Int) {
        val chapter = getItem(position)

        if (chapter != null) {
            when (holder.itemViewType) {
                VIEW_TYPE_HORIZONTAL_CARD_LIST -> (holder as ChapterHorizontalListViewHolder).bind(
                    chapter
                )
                else -> (holder as ChapterHorizontalListViewHolder).bind(chapter)
            }
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): RecyclerView.ViewHolder {
        return when (viewType) {
            VIEW_TYPE_HORIZONTAL_CARD_LIST -> {
                ChapterHorizontalListViewHolder(
                    ItemChapterHorizontalCardListBinding
                        .inflate(LayoutInflater.from(parent.context), parent, false)
                )
            }
            else -> ChapterHorizontalListViewHolder(
                ItemChapterHorizontalCardListBinding
                    .inflate(LayoutInflater.from(parent.context), parent, false)
            )
        }
    }

    class ChapterHorizontalListViewHolder(
        private val binding: ItemChapterHorizontalCardListBinding
    ) : RecyclerView.ViewHolder(binding.root) {
        init {
            binding.setClickListener { view ->
                binding.chapter?.let { chapter ->
                    navigateToChapterContent(chapter, view)
                }
            }
        }

        private fun navigateToChapterContent(chapter: DataChapter, view: View) {
            val direction =
                ChapterListFragmentDirections
                    .actionChapterListFragmentToChapterContentFragment(
                        chapterId = chapter.id,
                        bookId = chapter.bookId,
                        order = chapter.order
                    )
            view.findNavController().navigate(direction)
        }

        fun bind(item: DataChapter) {
            binding.apply {
                chapter = item
                executePendingBindings()
            }
        }
    }

}

private class DataChapterDiffCallback : DiffUtil.ItemCallback<DataChapter>() {
    override fun areItemsTheSame(oldItem: DataChapter, newItem: DataChapter): Boolean {
        return oldItem.id == newItem.id
    }

    override fun areContentsTheSame(oldItem: DataChapter, newItem: DataChapter): Boolean {
        return oldItem == newItem
    }
}
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
import com.ctosnetwork.qtreader.api.data.DataChapterSearch
import com.ctosnetwork.qtreader.databinding.ItemChapterSearchHorizontalCardListBinding
import com.ctosnetwork.qtreader.ui.chapter.ChapterSearchFragmentDirections

class ChapterSearchAdapter(
    private val viewType: Int
) : PagingDataAdapter<DataChapterSearch, RecyclerView.ViewHolder>(DataChapterSearchDiffCallback()) {

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
                VIEW_TYPE_HORIZONTAL_CARD_LIST -> (holder as ChapterSearchHorizontalListViewHolder).bind(
                    item
                )
                else -> (holder as ChapterSearchHorizontalListViewHolder).bind(item)
            }
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): RecyclerView.ViewHolder {
        return when (viewType) {
            VIEW_TYPE_HORIZONTAL_CARD_LIST -> {
                ChapterSearchHorizontalListViewHolder(
                    ItemChapterSearchHorizontalCardListBinding.inflate(
                        LayoutInflater.from(parent.context),
                        parent,
                        false
                    )
                )
            }
            else -> ChapterSearchHorizontalListViewHolder(
                ItemChapterSearchHorizontalCardListBinding.inflate(
                    LayoutInflater.from(parent.context),
                    parent,
                    false
                )
            )
        }
    }

    class ChapterSearchHorizontalListViewHolder(
        private val binding: ItemChapterSearchHorizontalCardListBinding
    ) : RecyclerView.ViewHolder(binding.root) {
        init {
            binding.setClickBookListener { view ->
                binding.chapterSearch?.let { chapterSearch ->
                    navigateToBookDetail(chapterSearch, view)
                }
            }
            binding.setClickChapterListener { view ->
                binding.chapterSearch?.let { chapterSearch ->
                    navigateToChapterContent(chapterSearch, view)
                }
            }
        }

        private fun navigateToBookDetail(item: DataChapterSearch, view: View) {
            val direction =
                ChapterSearchFragmentDirections.actionChapterSearchFragmentToBookDetailFragment(item.bookResult.id)
            view.findNavController().navigate(direction)
        }

        private fun navigateToChapterContent(item: DataChapterSearch, view: View) {
            val direction =
                ChapterSearchFragmentDirections.actionChapterSearchFragmentToChapterContentFragment(
                    chapterId = item.chapterResult.id,
                    bookId = item.bookResult.id,
                    order = 0
                )
            view.findNavController().navigate(direction)
        }

        fun bind(item: DataChapterSearch) {
            binding.apply {
                chapterSearch = item
                executePendingBindings()

                val content = item.contentHighlight.toString()
                val contentSub = content.substring(1, content.length - 1)
                itemChapterSearchContent.text = Html.fromHtml(contentSub, Html.FROM_HTML_MODE_LEGACY)

                val chapterTotalText = item.bookResult.chapterTotal.toString() + " chương"
                binding.itemChapterSearchChapter.text = chapterTotalText

                Glide.with(root)
                    .load(item.bookResult.cover)
                    .diskCacheStrategy(DiskCacheStrategy.NONE)
                    .placeholder(R.drawable.qd_book_cover)
                    .into(binding.itemChapterSearchCover)
            }


        }
    }


}

private class DataChapterSearchDiffCallback : DiffUtil.ItemCallback<DataChapterSearch>() {
    override fun areItemsTheSame(oldItem: DataChapterSearch, newItem: DataChapterSearch): Boolean {
        return oldItem.chapterResult.id == newItem.chapterResult.id
    }

    override fun areContentsTheSame(
        oldItem: DataChapterSearch,
        newItem: DataChapterSearch
    ): Boolean {
        return oldItem == newItem
    }
}
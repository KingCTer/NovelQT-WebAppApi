/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 13:37, 22/10/2021
 */

package com.ctosnetwork.qtreader.adapters

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.navigation.findNavController
import androidx.recyclerview.widget.DiffUtil
import androidx.recyclerview.widget.ListAdapter
import androidx.recyclerview.widget.RecyclerView
import com.bumptech.glide.Glide
import com.bumptech.glide.load.engine.DiskCacheStrategy
import com.ctosnetwork.qtreader.R
import com.ctosnetwork.qtreader.data.ImageSlider
import com.ctosnetwork.qtreader.databinding.PagerSliderItemBinding
import com.ctosnetwork.qtreader.ui.library.LibraryFragmentDirections

class ImageSliderAdapter : ListAdapter<ImageSlider, RecyclerView.ViewHolder>(ImageSliderDiffCallback()) {

    class  ImageSliderViewHolder(
        private val binding: PagerSliderItemBinding
    ) : RecyclerView.ViewHolder(binding.root){
        init {
            binding.setClickListener { view ->
                binding.imageSlider?.let { book ->
                    navigateToBookDetail(book, view)
                }
            }
        }

        private fun navigateToBookDetail(book: ImageSlider, view: View) {
            val direction =
                LibraryFragmentDirections.actionNavigationBookCardToBookDetailFragment(book.bookId)
            view.findNavController().navigate(direction)
        }

        fun bind(item: ImageSlider) {
            binding.apply {
                imageSlider = item
                executePendingBindings()
            }

            Glide.with(binding.root)
                .load(item.imageUrl)
                .diskCacheStrategy(DiskCacheStrategy.ALL)
                .placeholder(R.drawable.qd_toolbar_image)
                .into(binding.imageViewSliderItem)
        }


    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): RecyclerView.ViewHolder {
        return ImageSliderViewHolder(PagerSliderItemBinding.inflate(LayoutInflater.from(parent.context), parent, false))
    }

    override fun onBindViewHolder(holder: RecyclerView.ViewHolder, position: Int) {
        val imageSlider = getItem(position)
        (holder as ImageSliderViewHolder).bind(imageSlider)
    }

}

private class ImageSliderDiffCallback : DiffUtil.ItemCallback<ImageSlider>() {

    override fun areItemsTheSame(oldItem: ImageSlider, newItem: ImageSlider): Boolean {
        return oldItem.id == newItem.id
    }

    override fun areContentsTheSame(oldItem: ImageSlider, newItem: ImageSlider): Boolean {
        return oldItem == newItem
    }
}
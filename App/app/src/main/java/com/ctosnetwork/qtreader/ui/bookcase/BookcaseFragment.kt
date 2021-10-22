/*
 * Copyright (c) 2021 ctOS-Network. All rights reserved.
 * Created by KingCTer on 12:17, 22/10/2021
 */

package com.ctosnetwork.qtreader.ui.bookcase

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.TextView
import androidx.fragment.app.Fragment
import androidx.lifecycle.Observer
import androidx.lifecycle.ViewModelProvider
import com.ctosnetwork.qtreader.databinding.FragmentBookcaseBinding

class BookcaseFragment : Fragment() {

    private lateinit var bookcaseViewModel: BookcaseViewModel
    private var _binding: FragmentBookcaseBinding? = null

    // This property is only valid between onCreateView and
    // onDestroyView.
    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        bookcaseViewModel =
            ViewModelProvider(this).get(BookcaseViewModel::class.java)

        _binding = FragmentBookcaseBinding.inflate(inflater, container, false)
        val root: View = binding.root

        val textView: TextView = binding.textDashboard
        bookcaseViewModel.text.observe(viewLifecycleOwner, Observer {
            textView.text = it
        })
        return root
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}
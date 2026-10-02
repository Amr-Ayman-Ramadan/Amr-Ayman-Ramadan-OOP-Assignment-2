# LeetCode 1456 — Maximum Number of Vowels in a Substring of Given Length

Problem: https://leetcode.com/problems/maximum-number-of-vowels-in-a-substring-of-given-length

My accepted submission: https://leetcode.com/problems/maximum-number-of-vowels-in-a-substring-of-given-length/submissions/XXXXXXXXXX/

## Approach — Sliding Window
1. Count the vowels in the first window of size `k`.
2. Slide the window one step at a time: add 1 if the entering character is a vowel, subtract 1 if the leaving character is a vowel.
3. Keep the maximum count seen (stop early if it reaches `k`).

Time: O(n) — every character enters and leaves the window once. Space: O(1).

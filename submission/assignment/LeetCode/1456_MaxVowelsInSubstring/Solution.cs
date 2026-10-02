// LeetCode 1456. Maximum Number of Vowels in a Substring of Given Length
// Technique: fixed-size Sliding Window — O(n) time, O(1) extra space.
public class Solution
{
    public int MaxVowels(string s, int k)
    {
        // 1) Count the vowels of the first window s[0..k-1] once.
        int count = 0;
        for (int i = 0; i < k; i++)
        {
            if (IsVowel(s[i])) count++;
        }

        int max = count;

        // 2) Slide the window one character at a time:
        //    s[right] enters the window, s[right - k] leaves it.
        for (int right = k; right < s.Length; right++)
        {
            if (IsVowel(s[right])) count++;        // entering character
            if (IsVowel(s[right - k])) count--;    // leaving character

            if (count > max) max = count;
            if (max == k) return k;                // can't do better than all vowels
        }

        return max;
    }

    private static bool IsVowel(char c)
    {
        return c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u';
    }
}

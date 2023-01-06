using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelLoader
{
    public static void LoadNextLevel()
    {
        var sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        SceneManager.LoadScene(sceneIndex);
        PlayerData.Level++;
        
        
    }

    private static void FillArrayRandomNumbers(this IList<int> collection, int min, int max)
    {
        for (int i = 0; i < collection.Count; i++)
        {
            var number = Random.Range(min, max);
            
            while(collection.Contains(number))
                number = Random.Range(min, max);

            collection[i] = number;
        }
    }
}
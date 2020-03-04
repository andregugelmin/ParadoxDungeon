using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterStats : MonoBehaviour
{
    [HideInInspector]
    public int[] maxLife = new int[31], defenceList = new int[31], atackList = new int[31], experienceList = new int[31];   
    public int maxLifePlayer, experiencePlayer, Attack, Defence, Experience;
    public float Life, regenLife, regenCooldown = 1, timeRegenered;
    public int level;
    [HideInInspector]
    public int pointsXp, lifePoint, attackPoint, speedPoint, defencePoint;
    public CharClass charClass;
    public enum CharClass
    {
        goblin, caveira, estatua, player
    }

    [SerializeField]
    private ParticleSystem lvlupParticles;

    private void Start()
    {
        maxLife[0] = 4;
        for (int i = 1; i < 31; i++)
        {
            if (i % 5 != 0)
            {
                maxLife[i] = maxLife[i - 1] + 1;
            }
            else
            {
                maxLife[i] = maxLife[i - 1] + 2;
            }
            experienceList[i] = i;
        }
        switch (charClass)
        {
            case CharClass.player:
                maxLifePlayer = 10;
                Life = maxLifePlayer;
                Attack = 3;
                Defence = 1;
                break;
            case CharClass.caveira:
                for(int i = 0; i < 31; i++)
                {
                    atackList[i] = i;
                    defenceList[i] = i;
                }
                Life = maxLife[level];
                Attack = atackList[level];
                Defence = defenceList[level];
                gameObject.name = "Skeleton";
                break;
            case CharClass.goblin:
                atackList[0] = 3;
                defenceList[0] = 1;
                for (int i = 1; i < 31; i++)
                {
                    if (i % 2 == 0)
                    {
                        atackList[i] = atackList[i - 1];
                        defenceList[i] = defenceList[i - 1] + 1;
                    }
                    else
                    {
                        atackList[i] = atackList[i - 1] + 1;
                        defenceList[i] = defenceList[i - 1];
                    }
                }
                Life = maxLife[level];
                Attack = atackList[level];
                Defence = defenceList[level];
                gameObject.GetComponent<EnemyAnimation>().health = Life;
                gameObject.name = "Magmadar";
                break;
            case CharClass.estatua:
                atackList[0] = 1;
                defenceList[0] = 3;
                for (int i = 1; i < 31; i++)
                {
                    if (i < 4)
                    {
                        atackList[i] = 1;
                        defenceList[i] = i + 2;
                    }
                    else if((i-1)%5 == 0)
                    {
                        atackList[i] = atackList[i - 1] + 2;
                        defenceList[i] = defenceList[i - 1];
                    }
                    else
                    {
                        atackList[i] = atackList[i - 1];
                        defenceList[i] = defenceList[i - 1] + 1;
                    }
                }
                Life = maxLife[level];
                Attack = atackList[level];
                Defence = defenceList[level];
                gameObject.name = "Eyebat";
                break;
        }
        

        Experience = experienceList[level];
        
    }
    private void Update()
    {
        switch (charClass)
        {
            case CharClass.player:               

                if (experiencePlayer >= (level * 10))
                {
                    level += 1;
                    experiencePlayer = 0;
                    pointsXp += 3;
                    Life = maxLifePlayer;
                    lvlupParticles.Play();                    
                }
                if (Life < maxLifePlayer)
                {
                    if((Time.time - timeRegenered) > regenCooldown)
                    {
                        RegenLife();
                    }
                    
                }

                if(level >= 40)
                {
                    SceneManager.LoadScene("End");
                }
                                
                break;
        }
         
    }

    public void UpStats(string stat)
    {
        if (pointsXp > 0)
        {
            switch (stat)
            {
                case "Life":
                    Life += 0.4f;
                    regenLife += 0.1f;
                    lifePoint += 1;
                    pointsXp -= 1;
                    break;
                case "Attack":
                    Attack += 1;
                    pointsXp -= 1;
                    attackPoint += 1;
                    break;
                case "Defence":
                    Defence += 1;
                    pointsXp -= 1;
                    defencePoint += 1;
                    break;
            }
        }

    }

    void RegenLife()
    {
        Life += regenLife;
        timeRegenered = Time.time;
    }
}

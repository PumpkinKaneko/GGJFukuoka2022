using ExitGames.Client.Photon;

public class CharacterCustomInfo
{
    public string userId;
    public int hp;
    public bool isDead;
    public bool isBurning;
    public bool isBurned;
    public bool isAttack;
    
    public static CharacterCustomInfo HashTableToCharacterInfo(Hashtable table)
    {
        CharacterCustomInfo info = new CharacterCustomInfo();
        info.hp = (int)table["hp"];
        info.isDead = (bool) table["isDead"];
        info.isBurning = (bool) table["isBurning"];
        info.isBurned = (bool) table["isBurned"];
        info.isAttack = (bool) table["isAttack"];
        return info;
    }
    
}

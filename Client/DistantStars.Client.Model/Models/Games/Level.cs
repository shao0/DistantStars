namespace DistantStars.Client.Model.Models.Games
{
    public class Level
    {

        /// <summary>
        /// 级别
        /// </summary>
        public int LevelValue;
        /// <summary>
        /// 正确增加时间
        /// </summary>
        public double AddTime;
        /// <summary>
        /// 标志个数
        /// </summary>
        public int TagNumber;
        /// <summary>
        /// 系数
        /// </summary>
        public int Modulus => 256 / TagNumber;

        public Level()
        {
            Reset();
        }
        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            LevelValue = 1;
            AddTime = 10;
            TagNumber = 8;
        }
        /// <summary>
        /// 下一关
        /// </summary>
        public void NextLevel()
        {
            LevelValue++;
            if (LevelValue < 6 && LevelValue % 2 == 1)
            {
                if (TagNumber < 32) TagNumber *= 2;
            }
            else if (AddTime > 0)
            {
                
                AddTime -= 2;
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistantStars.Client.Model.Models.Games;

namespace DistantStars.Client.GameModule.Common
{
    public static class BlockCommon
    {
        #region 判断消除
        //垂直 X 相等  取 最小y -> 最大y 遍历，判断 value是否为0  遇到不是0返回false
        public static bool Vertical(this IReadOnlyList<IReadOnlyList<Block>> dataSourceList, Block a, Block b)
        {
            if (a.X != b.X) //垂直遍历 x必须相同
            {
                return false;
            }

            int yMax = Math.Max(a.Y, b.Y) - 1; //忽略自己
            int yMin = Math.Min(a.Y, b.Y) + 1; //从相邻的开始， 忽略自己

            for (int i = yMin; i <= yMax; i++)
            {
                if (dataSourceList[a.X][i].Tag != 0) //0代表 空
                {
                    return false;
                }
            }

            return true;
        }
        //水平 y 相等  取 最小x -> 最大X 遍历，判断 value是否为0  遇到不是0返回false
        public static bool Horizontal(this IReadOnlyList<IReadOnlyList<Block>> dataSourceList, Block a, Block b)
        {
            if (a.Y != b.Y) //垂直遍历 x必须相同
            {
                return false;

            }
            int xMax = Math.Max(a.X, b.X) - 1; //忽略自己
            int xMin = Math.Min(a.X, b.X) + 1; //从相邻的开始， 忽略自己

            for (int i = xMin; i <= xMax; i++)
            {
                if (dataSourceList[i][a.Y].Tag != 0) //0代表 空
                {
                    return false;
                }
            }

            return true;
        }
        public static bool OnePoint(this IReadOnlyList<IReadOnlyList<Block>> dataSourceList, Block a, Block b, out Block[] pathPoints)
        {

            Block pointC = new Block() { X = a.X, Y = b.Y }; //x 和 a相同  ，y和b相同
            Block pointD = new Block() { X = b.X, Y = a.Y };
            pathPoints = null;
            bool result = false;
            if (dataSourceList[pointC.X][pointC.Y].Tag == 0)
            {
                pathPoints = new[] { a, pointC, b };
                result = dataSourceList.Vertical(a, pointC) && dataSourceList.Horizontal(pointC, b);
            }
            if (!result && dataSourceList[pointD.X][pointD.Y].Tag == 0)
            {
                pathPoints = new[] { a, pointD, b };
                result = dataSourceList.Horizontal(a, pointD) && dataSourceList.Vertical(pointD, b); //从 a-->c 再 从c-->b
            }
            return result;

        }
        public static bool TwoPointPath(this IReadOnlyList<IReadOnlyList<Block>> dataSourceList, Block a, Block b, out Block[] pathPoints)
        {
            pathPoints = null;
            //找一点 满足 
            for (int i = 0; i < dataSourceList.Count; i++)
            {
                for (int j = 0; j < dataSourceList[i].Count; j++)
                {
                    if (a.X == i && a.Y == j || b.X == i && b.Y == j) continue;
                    //if (i != a.X && i != b.X && j != a.Y && j != b.Y)
                    if (dataSourceList[i][j].Tag == 0)
                    {
                        // 这个点 是a的一点拐点，且是 b的 垂直或者水平点
                        //或者这个点是 b的一点拐点， 且 是 a的垂直点 或者水平点
                        var c = new Block() { Y = j, X = i };
                        if (dataSourceList.OnePoint(a, c, out pathPoints) && (dataSourceList.Vertical(c, b) || dataSourceList.Horizontal(c, b)) || dataSourceList.OnePoint(c, b, out pathPoints) && (dataSourceList.Vertical(a, c) || dataSourceList.Horizontal(a, c)))
                        {
                            var blocks = pathPoints.ToList();
                            blocks.Insert(0, a);
                            blocks.Add(b);
                            pathPoints = blocks.ToArray();
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        public static bool BlockMatch(this IReadOnlyList<IReadOnlyList<Block>> dataSourceList, Block a, Block b, out Block[] pathPoints)
        {

            bool isMatch = false;
            pathPoints = null;
            if (a.X == b.X || a.Y == b.Y)  //处理  位置 处于  十字线情况  不存在 一拐点
            {
                isMatch = a.X == b.X ? dataSourceList.Vertical(a, b) : dataSourceList.Horizontal(a, b);
                if (isMatch)
                {
                    pathPoints = new[] { a, b };
                    return isMatch;
                }


                isMatch = dataSourceList.TwoPointPath(a, b, out pathPoints);

                return isMatch;
            }

            isMatch = dataSourceList.OnePoint(a, b, out pathPoints);
            if (isMatch)
            {
                return isMatch;
            }

            isMatch = dataSourceList.TwoPointPath(a, b, out pathPoints);

            return isMatch;
        }
        #endregion
    }
}

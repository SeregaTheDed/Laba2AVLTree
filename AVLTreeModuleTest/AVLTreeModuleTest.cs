using AVLTree;

namespace AVLTreeModuleTest
{
    [TestClass]
    public class AVLTreeUnitTest
    {
        const int n = 100000;

        [TestMethod]
        public void TestAddCountIncrement()
        {
            AVLTree<string, int> tree = new AVLTree<string, int>();
            tree.Add("abc", 123);
            Assert.AreEqual(1, tree.Count);
        }

        [TestMethod]
        public void TestAddCountIncrementTwice()
        {
            AVLTree<string, int> tree = new AVLTree<string, int>();
            tree.Add("abc", 123);
            tree.Add("abcd", 1234);
            Assert.AreEqual(2, tree.Count);
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestAddExceptionIfAddEqualsKeys()
        {
            AVLTree<string, int> tree = new AVLTree<string, int>();
            tree.Add("abc", 123);
            tree.Add("abc", 123);
        }

        [TestMethod]
        public void TestGetGettedValueEqualsAddedValue()
        {
            AVLTree<string, int> tree = new AVLTree<string, int>();
            tree.Add("abc", 123);
            Assert.AreEqual(123, tree["abc"]);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void TestGetExceptionIfTryGetNotExistsKey()
        {
            AVLTree<string, int> tree = new AVLTree<string, int>();
            var abc = tree["abc"];
        }

        [TestMethod]
        public void TestRemoveCountEqualZeroAfterAddAndRemoveOneElement()
        {
            AVLTree<string, int> tree = new AVLTree<string, int>();
            tree.Add("abc", 123);
            tree.Remove("abc");
            Assert.AreEqual(0, tree.Count);
        }

        [TestMethod]
        public void TestRemoveCountEqualZeroAfterAddAndRemoveSomeElements()
        {
            AVLTree<string, int> tree = new AVLTree<string, int>();
            for (int i = 0; i < n; i++)
            {
                tree.Add(i.ToString(), i);
            }
            for (int i = 0; i < n; i++)
            {
                tree.Remove(i.ToString());
            }
            Assert.AreEqual(0, tree.Count);
        }

        [TestMethod]
        public void TestContainsWorkAtSimpleExample()
        {
            AVLTree<string, int> tree = new AVLTree<string, int>();
            tree.Add("abc", 123);
            Assert.IsTrue(tree.Contains("abc"));
        }

        [TestMethod]
        public void TestContainsWorkAtMediumExample()
        {
            AVLTree<string, int> tree = new AVLTree<string, int>();
            for (int i = 0; i < n; i++)
            {
                tree.Add(i.ToString(), i);
            }
            Assert.IsTrue(tree.Contains( (n/2).ToString() ));
        }

        [TestMethod]
        public void TestContainsWorkAtHardExample()
        {
            AVLTree<string, int> tree = new AVLTree<string, int>();
            int GuessedKeys = 0;
            for (int i = 0; i < n; i++)
            {
                tree.Add(i.ToString(), i);
            }
            for (int i = 0; i < n; i++)
            {
                if (tree.Contains(i.ToString()))
                    GuessedKeys++;
            }
            Assert.AreEqual(n, GuessedKeys);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestSuite
    {
        // From https://www.raywenderlich.com/9454-introduction-to-unity-unit-testing
        private Game game;

        [SetUp]
        public void Setup()
        {
            // 2: Creates an instance of the Game 
            GameObject gameGameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Game"));
            game = gameGameObject.GetComponent<Game>();
        }

        [TearDown]
        public void Teardown()
        {
            // 7:  Clean up the game object
            Object.Destroy(game.gameObject);
        }

        // 1: This is an attribute
        [UnityTest]
        public IEnumerator AsteroidsMoveDown()
        {

            // 3: Creates an Asteroid using the Spawn Asteroid Method
            GameObject asteroid = game.GetSpawner().SpawnAsteroid();

            // 4: Gets the inital asteroid y position
            float initialYPos = asteroid.transform.position.y;

            // 5: This coroutine must have a yeild return, in this case we wait for one second but it can also return null
            yield return new WaitForSeconds(0.1f);

            // 6: We assert that the position of the asteroid will be less than the initial position
            Assert.Less(asteroid.transform.position.y, initialYPos);
        }

        [UnityTest]
        public IEnumerator GameOverOccursOnAsteroidCollision()
        {
            GameObject asteroid = game.GetSpawner().SpawnAsteroid();

            // 1: move the asteroid to the same position as the ship causing a game over
            asteroid.transform.position = game.GetShip().transform.position;

            // 2: wait for .1 seconds to ensure that the collsion event occurs
            yield return new WaitForSeconds(0.1f);

            // 3: Checks to ensure that the isGameOver flag is set to true.
            Assert.True(game.isGameOver);

        }

        [UnityTest]
        public IEnumerator NewGameRestartsGame()
        {
            // 1: set isGameOver to true
            game.isGameOver = true;

            // 1.5: The NewGame() method should set isGameOver to false
            game.NewGame();

            // 2: Assert that isGameOver will be false 
            Assert.False(game.isGameOver);
            yield return null;
        }

        [UnityTest]
        public IEnumerator LaserMovesUp()
        {
            // 1: Gets a reference to a laser spawned from the ship
            GameObject laser = game.GetShip().SpawnLaser();

            // 2: initial position recorded
            float initialYPos = laser.transform.position.y;
            yield return new WaitForSeconds(0.1f);

            // 3: after waiting check that the laser is in a position greater than the initial position
            Assert.Greater(arg1: laser.transform.position.y,arg2: initialYPos);
        }

        [UnityTest]
        public IEnumerator LaserDestroysAsteroid()
        {
            // 1: Create an asteroid and a laser in the same location to trigger a collsion
            GameObject asteroid = game.GetSpawner().SpawnAsteroid();
            asteroid.transform.position = Vector3.zero;
            GameObject laser = game.GetShip().SpawnLaser();
            laser.transform.position = Vector3.zero;
            yield return new WaitForSeconds(0.1f);

            // 2: To check for Unity nulls you have to use UnityEngine.Assertions.Assert.IsNull instead of the regular Assert.IsNull
            UnityEngine.Assertions.Assert.IsNull(asteroid);
        }

        [UnityTest]
        public IEnumerator DestroyedAsteroidIncreasesScore()
        {
            // 1: Spawn an asteroid and laser in the same position to ensure that a collision occurs which should cause a score increase
            GameObject asteroid = game.GetSpawner().SpawnAsteroid();
            asteroid.transform.position = Vector3.zero;
            GameObject laser = game.GetShip().SpawnLaser();
            laser.transform.position = Vector3.zero;
            yield return new WaitForSeconds(0.1f);

            // 2: Check to make sure the score equals 1 instead of zero
            Assert.AreEqual(game.score, 1);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void TestSuiteSimplePasses()
        {
            // Use the Assert class to test conditions
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestSuiteWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}

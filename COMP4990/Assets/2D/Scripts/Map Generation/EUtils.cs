using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// Some classes and methods commonly found within the project.
/// </summary>
public class EUtils : MonoBehaviour
{
    /// <summary>
    /// Stores the position and tile indexes of a chunk.
    /// </summary>
    public class Chunk
    {
        public int X {get; set;}
        public int Y {get; set;}
        public int[,] ChunkTiles {get; set;}

        /// <summary>
        /// Creates a new chunk object with the given parameters.
        /// </summary>
        /// <param name="x">X coordinate of the chunk</param>
        /// <param name="y">Y coordinate of the chunk</param>
        /// <param name="chunkSize">Length and width of the chunk in tiles</param>
        /// <remarks>
        /// <para>
        /// When a new chunk is created, the ChunkTiles array is 
        /// initialized to -1 to represent empty space.
        /// </para>
        /// </remarks>
        public Chunk(int x, int y, int chunkSize)
        {
            X = x;
            Y = y;
            ChunkTiles = new int[chunkSize, chunkSize];

            // Initialize chunkTiles to -1 (no tile has been placed here yet)
            for(int i = 0; i < chunkSize; i++)
            {
                for(int j = 0; j < chunkSize; j++)
                {
                    ChunkTiles[i, j] = -1;
                }
            }
        }

        /// <summary>
        /// Returns the chunk key of a given chunk object.
        /// </summary>
        /// <param name="chunk">The chunk object</param>
        /// <returns>A string with the chunk X and Y.</returns>
        public string GetKey(Chunk chunk)
        {
            return GetChunkKey(chunk.X, chunk.Y);
        }

        /// <summary>
        /// Returns the chunk key of the current chunk object.
        /// </summary>
        /// <returns>A string with the chunk X and Y.</returns>
        public string GetKey()
        {
            return GetChunkKey(X, Y);
        }
    }

    /// <summary>
    /// Stores the type, position and tile index of a biome.
    /// </summary>
    public class EBiome
    {
        public BiomeEnum Type {get; set;}
        public int Cx {get; set;}
        public int Cy {get; set;}
        public int Tx {get; set;}
        public int Ty {get; set;}
        public int TileIndex {get; set;}
        public int BiomeIndex {get; set;}

        /// <summary>
        /// Creates a new biome object with the given parameters.
        /// </summary>
        /// <param name="type">Type of biome</param>
        /// <param name="cx">Chunk X</param>
        /// <param name="cy">Chunk Y</param>
        /// <param name="tx">Tile X</param>
        /// <param name="ty">Tile Y</param>
        /// <param name="tileIndex">Index of the tile used to paint the biome, see <see cref="global::TileIndex"/></param>
        /// <param name="biomeIndex">Each biome has a unique index</param>
        public EBiome(BiomeEnum type, int cx, int cy, int tx, int ty, int tileIndex, int biomeIndex)
        {
            Type = type;
            Cx = cx;
            Cy = cy;
            Tx = tx;
            Ty = ty;
            TileIndex = tileIndex;
            BiomeIndex = biomeIndex;
        }

        /// <summary>
        /// Returns the biome key of the current biome.
        /// </summary>
        /// <returns>A string in the format of <see cref="GetCoordinateKey"/>.</returns>
        public string GetBiomeKey()
        {
            return $"C({Cx}, {Cy} : T({Tx}, {Ty}))";
        }

        /// <summary>
        /// Returns the biome type of the current biome.
        /// </summary>
        /// <returns>A BiomeEnum</returns>
        public BiomeEnum GetBiomeType()
        {
            return Type;
        }
    }

    //TODO: make tree, rock and cactus a class that implements an interface

    /// <summary>
    /// Stores the position and object index of a tree.
    /// </summary>
    public class Tree
    {
        public int Index {get; set;}
        public int Cx {get; set;}
        public int Cy {get; set;}
        public int Tx {get; set;}
        public int Ty {get; set;}

        /// <summary>
        /// Create a new tree object with the given parameters.
        /// </summary>
        /// <param name="index">Index of the tree, see <see cref="global::TileIndex"/></param>
        /// <param name="cx">Chunk X</param>
        /// <param name="cy">Chunk Y</param>
        /// <param name="tx">Tile X</param>
        /// <param name="ty">Tile Y</param>
        public Tree(int index, int cx, int cy, int tx, int ty)
        {
            Index = index;
            Cx = cx;
            Cy = cy;
            Tx = tx;
            Ty = ty;
        }
    }

    public class RockCluster
    {
        public int children {get; set;}

        public RockCluster()
        {
            children = 0;
        }

        public void addChild()
        {
            children++;
        }

        public void removeChild()
        {
            children--;
        }
    }

    /// <summary>
    /// Stores the position and object index of a rock.
    /// </summary>
    public class Rock
    {
        public int Index {get; set;}
        public int Cx {get; set;}
        public int Cy {get; set;}
        public int Tx {get; set;}
        public int Ty {get; set;}
        public RockCluster ParentCluster {get; set;}

        /// <summary>
        /// Create a new rock object with the given parameters.
        /// </summary>
        /// <param name="index">Index of the rock, see <see cref="global::TileIndex"/>s</param>
        /// <param name="cx">Chunk X</param>
        /// <param name="cy">Chunk Y</param>
        /// <param name="tx">Tile X</param>
        /// <param name="ty">Tile Y</param>
        public Rock(int index, int cx, int cy, int tx, int ty, RockCluster parentCluster)
        {
            Index = index;
            Cx = cx;
            Cy = cy;
            Tx = tx;
            Ty = ty;
            ParentCluster = parentCluster;
            ParentCluster.addChild();
        }

        public void DestroyRock()
        {
            ParentCluster.removeChild();
        }

        public bool ClusterDestroyed()
        {
            if(ParentCluster.children <= 0)
            {
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Stores the position and object index of a cactus.
    /// </summary>
    public class Cactus
    {
        public int Index {get; set;}
        public int Cx {get; set;}
        public int Cy {get; set;}
        public int Tx {get; set;}
        public int Ty {get; set;}

        /// <summary>
        /// Create a new cactus object with the given parameters.
        /// </summary>
        /// <param name="index">Index of the cactus, see <see cref="global::TileIndex"/></param>
        /// <param name="cx">Chunk X</param>
        /// <param name="cy">Chunk Y</param>
        /// <param name="tx">Tile X</param>
        /// <param name="ty">Tile Y</param>
        public Cactus(int index, int cx, int cy, int tx, int ty)
        {
            Index = index;
            Cx = cx;
            Cy = cy;
            Tx = tx;
            Ty = ty;
        }
    }

    public class CPorcupine
    {
        public int X {get; set;}
        public int Y {get; set;}

        public CPorcupine(int x, int y)
        {
            X = y;
            Y = x;
        }
    }

    /// <summary>
    /// Stores the position, radius, required space and index of a structure.
    /// </summary>
    /// <remarks>
    /// <para>
    /// im not really sure to be honest, ill change how structures work later.
    /// </para>
    /// </remarks>
    public class Structure
    {
        public int Index {get; set;}
        public int XRad {get; set;}
        public int YUp {get; set;}
        public int YDown {get; set;}
        public int Cx {get; set;}
        public int Cy {get; set;}
        public int Tx {get; set;}
        public int Ty {get; set;}

        /// <summary>
        /// Create a new structure object with the given parameters.
        /// </summary>
        /// <param name="index">Index of the structure, see <see cref="TileIndex"/></param>
        /// <param name="xRad">X radius</param>
        /// <param name="yUp">Tiles up from the origin</param>
        /// <param name="yDown">Tiles down from the origin</param>
        public Structure(int index, int xRad, int yUp, int yDown)
        {
            Index = index;
            XRad = xRad;
            YUp = yUp;
            YDown = yDown;
        }
    }

    public class PlaceableBlock
    {
        public int PID {get; set;}
        public int X {get; set;}
        public int Y {get; set;}
        public int Z {get; set;}

        public PlaceableBlock(int pid, int x, int y, int z)
        {
            PID = pid;
            X = x;
            Y = y;
            Z = z;
        }
    }

    public class CInventoryItem
    {
        public int Idx;
        public int Count;

        public CInventoryItem(int idx, int count)
        {
            Idx = idx;
            Count = count;
        }
    }

    // something going on here fr
    public enum EAttackType
    {
        Swing,
        Stab
    }

    /// <summary>
    /// Biome enum.
    /// </summary>
    public enum BiomeEnum
    {
        Forest,
        Desert,
        Ocean
    }

    /// <summary>
    /// Map size enum.
    /// </summary>
    public enum MapSize
    {
        Small,
        Medium,
        Large
    }

    /// <summary>
    /// Menu state enum.
    /// </summary>
    public enum MenuState
    {
        Main,
        Options,
        Play,
        Quit,
        NewGame,
        LoadGame,
        ConfirmLoadGame
    }

    public enum ETool
    {
        Pickaxe,
        Axe
    }

    public enum EDamageType
    {
        Melee,
        Ranged,
        Magic
    }

    public enum ESapling
    {
        Tree,
        Cactus
    }

    /// <summary>
    /// not used. will remove later.
    /// </summary>
    public enum BBTKey
    {
        Null,
        BlendLeft,
        BlendRight,
        BlendUp,
        BlendDown,
        BlendUpLeft,
        BlendUpRight,
        BlendDownLeft,
        BlendDownRight
    }

    /// <summary>
    /// A collection of data to be serialized and stored when saving or loading a world.
    /// </summary>
    public class WorldData
    {
        public int Seed {get; set;}
        public MapSize WorldSize {get; set;}
        public int PlayerX {get; set;}
        public int PlayerY {get; set;}
        public Dictionary<string, Chunk> AboveGroundChunks {get; set;}
        public Dictionary<string, Chunk> GroundChunks {get; set;}
        public Dictionary<string, Chunk> WaterChunks {get; set;}
        public Dictionary<string, Tree> Trees {get; set;}
        public Dictionary<string, Rock> Rocks {get; set;}
        public Dictionary<string, Cactus> Cacti {get; set;}
        public List<Structure> Structures {get; set;}
        public List<CPorcupine> Porcupines {get; set;}
        public int PorcupinesPlaced {get; set;}
        public int PorcupineCap {get; set;}
        public DayNightCycle.CycleState DayNightState {get; set;}
        public float DayNightStateTimer {get; set;}
        public float DayNightAlpha {get; set;}
        public List<PlaceableBlock> PlaceableBlocks {get; set;}
        public List<CInventoryItem> InventoryItems {get; set;}
    }

    /// <summary>
    /// Returns the coordinate key based on the given parameters.
    /// </summary>
    /// <param name="cx">Chunk X</param>
    /// <param name="cy">Chunk Y</param>
    /// <param name="tx">Tile X</param>
    /// <param name="ty">Tile Y</param>
    /// <returns>A string with the chunk and tile coordinates.</returns>
    public static string GetCoordinateKey(int cx, int cy, int tx, int ty)
    {
        return $"C({cx}, {cy}) : T({tx}, {ty})";
    }

    /// <summary>
    /// Returns a chunk key based on the given parameters.
    /// </summary>
    /// <param name="cx">Chunk X</param>
    /// <param name="cy">Chunk Y</param>
    /// <returns>A string with the chunk coordinates.</returns>
    public static string GetChunkKey(int cx, int cy)
    {
        return $"C({cx}, {cy})";
    }

    /// <summary>
    /// Returns a Vector3Int converting chunk and tile positions to world pos.
    /// <seealso cref="WorldToChunkPos"/>
    /// </summary>
    /// <param name="cx">Chunk X</param>
    /// <param name="cy">Chunk Y</param>
    /// <param name="tx">Tile X</param>
    /// <param name="ty">Tile Y</param>
    /// <param name="chunkSize">Length and width of the chunk in tiles</param>
    /// <returns>A Vector3Int representing the world pos with z = 0.</returns>
    public static Vector3Int ChunkToWorldPos(int cx, int cy, int tx, int ty, int chunkSize)
    {
        return new Vector3Int(cx * chunkSize + tx , cy * chunkSize + ty, 0);
    }

    /// <summary>
    /// Returns a Tuple converting world pos to chunk and tile positions.
    /// <seealso cref="ChunkToWorldPos"/>
    /// </summary>
    /// <param name="wx"></param>
    /// <param name="wy"></param>
    /// <param name="chunkSize"></param>
    /// <returns>A Tuple representing chunk and tile positions (cx, cy, tx, ty).</returns>
    public static (int, int, int, int) WorldToChunkPos(int wx, int wy, int chunkSize)
    {
        int cx = (int)Math.Floor((double)wx / chunkSize);
        int cy = (int)Math.Floor((double)wy / chunkSize);

        int tx = (wx % chunkSize + chunkSize) % chunkSize;
        int ty = (wy % chunkSize + chunkSize) % chunkSize;
        
        return (cx, cy, tx, ty);
    }

    /// <summary>
    /// Returns true if a specified tile exists within the given array.
    /// not used currently.
    /// </summary>
    /// <param name="chunkTiles">ChunkTiles array</param>
    /// <param name="tile">Tile to search for</param>
    /// <returns>true if the tile exists in the array, false otherwise.</returns>
    public static bool TileInChunk(int[,] chunkTiles, int tile)
    {
        foreach(int t in chunkTiles)
        {
            if(t == tile)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Returns the distance between 2 points
    /// </summary>
    /// <param name="x1">X of point 1</param>
    /// <param name="y1">Y of point 1</param>
    /// <param name="x2">X of point 2</param>
    /// <param name="y2">Y of point 2</param>
    /// <returns>A double representing the distance between 2 points</returns>
    public static double GetDistance(int x1, int y1, int x2, int y2)
    {
        return Math.Abs(Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2)));
    }
}

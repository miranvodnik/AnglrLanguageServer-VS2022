
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AnglrBreakPointDBLibrary
{

    [Obfuscation (Exclude = true)]
    [JsonConverter (typeof (StringEnumConverter))]
    public enum AnglrBreakPointType
    {
        None = 0,
        Reduce = 1,
        Shift = 2,
        Goto = 3,
    }

    [Obfuscation (Exclude = true)]
    [JsonObject]
    public class AnglrBreakPoint
    {
        public AnglrBreakPointType Type { get; set; }
        public AnglrBreakPoint () { }
        public AnglrBreakPoint (AnglrBreakPointType type)
        {
            Type = type;
        }
    }

    [Obfuscation (Exclude = true)]
    [JsonObject]
    public class AnglrReduceBP : AnglrBreakPoint
    {
        public int ProductionNumber { get; set; }
        public AnglrReduceBP () : base (AnglrBreakPointType.Reduce) { }
    }

    [Obfuscation (Exclude = true)]
    [JsonObject]
    public class AnglrShiftBP : AnglrBreakPoint
    {
        public int TerminalCode { get; set; }
        public int InState { get; set; }
        public int OutState { get; set; }
        public AnglrShiftBP () : base (AnglrBreakPointType.Shift) { }
    }

    [Obfuscation (Exclude = true)]
    [JsonObject]
    public class AnglrGotoBP : AnglrBreakPoint
    {
        public int NonTerminalCode { get; set; }
        public int InState { get; set; }
        public int OutState { get; set; }
        public AnglrGotoBP () : base (AnglrBreakPointType.Goto) { }
    }

    [Obfuscation (Exclude = true)]
    public class cmprbp : IEqualityComparer<AnglrReduceBP>
    {
        public bool Equals (AnglrReduceBP x, AnglrReduceBP y) => x.ProductionNumber == y.ProductionNumber;
        public int GetHashCode (AnglrReduceBP obj) => obj.ProductionNumber;
    }

    [Obfuscation (Exclude = true)]
    public class cmpsbp : IEqualityComparer<AnglrShiftBP>
    {
        public bool Equals (AnglrShiftBP x, AnglrShiftBP y)
            => (x.TerminalCode == y.TerminalCode) &&
               (x.InState == y.InState) &&
               (x.OutState == y.OutState);

        public int GetHashCode (AnglrShiftBP obj)
            => obj.TerminalCode + obj.InState + obj.OutState;
    }

    [Obfuscation (Exclude = true)]
    public class cmpgbp : IEqualityComparer<AnglrGotoBP>
    {
        public bool Equals (AnglrGotoBP x, AnglrGotoBP y)
            => (x.NonTerminalCode == y.NonTerminalCode) &&
               (x.InState == y.InState) &&
               (x.OutState == y.OutState);

        public int GetHashCode (AnglrGotoBP obj)
            => obj.NonTerminalCode + obj.InState + obj.OutState;
    }

    [Obfuscation (Exclude = true)]
    [JsonObject]
    public class AnglrBreakPointDBChunk
    {
        public delegate void AnglrReduceBPDelegate (AnglrReduceBP bp, bool removed);
        public delegate void AnglrShiftBPDelegate (AnglrShiftBP bp, bool removed);
        public delegate void AnglrGotoBPDelegate (AnglrGotoBP bp, bool removed);

        public event AnglrReduceBPDelegate AnglrReduceBPEvent;
        public event AnglrShiftBPDelegate AnglrShiftBPEvent;
        public event AnglrGotoBPDelegate AnglrGotoBPEvent;

        public bool Changed { get; set; }
        public HashSet<AnglrReduceBP> AnglrReduceBPSet { get; set; }
            = new HashSet<AnglrReduceBP> (new cmprbp ());

        public HashSet<AnglrShiftBP> AnglrShiftBPSet { get; set; }
            = new HashSet<AnglrShiftBP> (new cmpsbp ());

        public HashSet<AnglrGotoBP> AnglrGotoBPSet { get; set; }
            = new HashSet<AnglrGotoBP> (new cmpgbp ());

        public void ApplyChanges (AnglrBreakPointDBChunk chunk)
        {
            if (chunk == null)
                return;
            foreach (var bp in AnglrReduceBPSet)
            {
                if (!chunk.AnglrReduceBPSet.Contains (bp))
                    AnglrReduceBPEvent?.Invoke (bp, true);  // bp has been removed
            }
            foreach (var bp in AnglrShiftBPSet)
            {
                if (!chunk.AnglrShiftBPSet.Contains (bp))
                    AnglrShiftBPEvent?.Invoke (bp, true);  // bp has been removed
            }
            foreach (var bp in AnglrGotoBPSet)
            {
                if (!chunk.AnglrGotoBPSet.Contains (bp))
                    AnglrGotoBPEvent?.Invoke (bp, true);  // bp has been removed
            }
            foreach (var bp in chunk.AnglrReduceBPSet)
            {
                if (!AnglrReduceBPSet.Contains (bp))
                    AnglrReduceBPEvent?.Invoke (bp, false);  // bp has been removed
            }
            foreach (var bp in chunk.AnglrShiftBPSet)
            {
                if (!AnglrShiftBPSet.Contains (bp))
                    AnglrShiftBPEvent?.Invoke (bp, false);  // bp has been removed
            }
            foreach (var bp in chunk.AnglrGotoBPSet)
            {
                if (!AnglrGotoBPSet.Contains (bp))
                    AnglrGotoBPEvent?.Invoke (bp, false);  // bp has been removed
            }
            AnglrReduceBPSet = chunk.AnglrReduceBPSet;
            AnglrShiftBPSet = chunk.AnglrShiftBPSet;
            AnglrGotoBPSet = chunk.AnglrGotoBPSet;
        }
    }

    [Obfuscation (Exclude = true)]
    [JsonObject]
    public static class AnglrBreakPointDB
    {
        public static Dictionary<int, AnglrBreakPointDBChunk> DB { get; set; }
            = new Dictionary<int, AnglrBreakPointDBChunk> ();

        private static readonly JsonSerializerSettings Settings =
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented
            };

        public static string AnglrBreakPointDBPath =>
            Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.LocalApplicationData),
                         ".anglr", "AnglrBreakPointDB.json");

        public static Exception AnglrBreakPointDBException { get; set; }

        public static bool Load ()
        {
            try
            {
                if (!File.Exists (AnglrBreakPointDBPath))
                    DB = new Dictionary<int, AnglrBreakPointDBChunk> ();
                else
                    DB = JsonConvert.DeserializeObject<Dictionary<int, AnglrBreakPointDBChunk>> (File.ReadAllText (AnglrBreakPointDBPath), Settings);
                AnglrBreakPointDBException = null;
                return true;
            }
            catch (Exception ex)
            {
                AnglrBreakPointDBException = ex;
                return false;
            }
        }

        public static bool Save ()
        {
            try
            {
                Directory.CreateDirectory (Path.GetDirectoryName (AnglrBreakPointDBPath));
                File.WriteAllText (AnglrBreakPointDBPath, JsonConvert.SerializeObject (DB, Settings));
                return true;
            }
            catch (Exception ex)
            {
                AnglrBreakPointDBException = ex;
                return false;
            }
        }

        public static AnglrBreakPointDBChunk Add (int magicNumber)
            => DB [magicNumber] = new AnglrBreakPointDBChunk ();

        public static bool Get (int magicNumber, out AnglrBreakPointDBChunk chunk)
            => DB.TryGetValue (magicNumber, out chunk);
    }
}

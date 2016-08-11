using System;
using System.Collections.Generic;
using IniParser;
using IniParser.Model;

namespace ihc
{
    class ProgramMapping
    {
        public string name { get; private set; }
        public string program_regex_str { get; private set; }
        public Dictionary<Types.ButtonFlags, Action> mappings { get; private set; }

        public ProgramMapping(string name, string program_regex_str, Dictionary<Types.ButtonFlags, Action> mappings)
        {
            this.name = name;
            this.program_regex_str = program_regex_str;
            this.mappings = mappings;
        }

        private static string readMapping(KeyDataCollection section, string key_name)
        {
            string key_value = section[key_name];
            if (key_value == "{SPACE}")
            {
                key_value = " ";
            }
            return key_value;
        }

        public static ProgramMapping Build(string name, string program_regex_str, Dictionary<Types.ButtonFlags, Action> mappings)
        {
            return new ProgramMapping(name, program_regex_str, mappings);
        }

        public static ProgramMapping BuildFromFile(string filename)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(filename);

            string name = data["ID"]["name"];
            string program_regex_str = data["PROGRAM"]["regex"];

            Dictionary<Types.ButtonFlags, Action> mappings = new Dictionary<Types.ButtonFlags, Action>();
            KeyDataCollection mapping_section = data["MAPPINGS"];
            KeyDataCollection labels_section = data["LABELS"];

            if (mapping_section.ContainsKey("START"))
            {
                mappings.Add(Types.ButtonFlags.START, new Action(readMapping(mapping_section, "START"), labels_section["START"]));
            }
            if (mapping_section.ContainsKey("BACK"))
            {
                mappings.Add(Types.ButtonFlags.BACK, new Action(mapping_section["BACK"], labels_section["BACK"]));
            }
            if (mapping_section.ContainsKey("A"))
            {
                mappings.Add(Types.ButtonFlags.A, new Action(mapping_section["A"], labels_section["A"]));
            }
            if (mapping_section.ContainsKey("B"))
            {
                mappings.Add(Types.ButtonFlags.B, new Action(mapping_section["B"], labels_section["B"]));
            }
            if (mapping_section.ContainsKey("X"))
            {
                mappings.Add(Types.ButtonFlags.X, new Action(readMapping(mapping_section, "X"), labels_section["X"]));
            }
            if (mapping_section.ContainsKey("Y"))
            {
                mappings.Add(Types.ButtonFlags.Y, new Action(mapping_section["Y"], labels_section["Y"]));
            }
            if (mapping_section.ContainsKey("LEFT_SHOULDER"))
            {
                mappings.Add(Types.ButtonFlags.LEFT_SHOULDER, new Action(mapping_section["LEFT_SHOULDER"], labels_section["LEFT_SHOULDER"]));
            }
            if (mapping_section.ContainsKey("RIGHT_SHOULDER"))
            {
                mappings.Add(Types.ButtonFlags.RIGHT_SHOULDER, new Action(mapping_section["RIGHT_SHOULDER"], labels_section["RIGHT_SHOULDER"]));
            }
            if (mapping_section.ContainsKey("DPAD_DOWN"))
            {
                mappings.Add(Types.ButtonFlags.DPAD_DOWN, new Action(mapping_section["DPAD_DOWN"], labels_section["DPAD_DOWN"]));
            }
            if (mapping_section.ContainsKey("DPAD_LEFT"))
            {
                mappings.Add(Types.ButtonFlags.DPAD_LEFT, new Action(mapping_section["DPAD_LEFT"], labels_section["DPAD_LEFT"]));
            }
            if (mapping_section.ContainsKey("DPAD_UP"))
            {
                mappings.Add(Types.ButtonFlags.DPAD_UP, new Action(mapping_section["DPAD_UP"], labels_section["DPAD_UP"]));
            }
            if (mapping_section.ContainsKey("DPAD_RIGHT"))
            {
                mappings.Add(Types.ButtonFlags.DPAD_RIGHT, new Action(mapping_section["DPAD_RIGHT"], labels_section["DPAD_RIGHT"]));
            }

            return ProgramMapping.Build(name, program_regex_str, mappings);
        }
    }
}

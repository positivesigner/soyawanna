using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASLSJParse
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.chkOnlyErrors.Checked = false;
            Format_SyllableList();
            if (this.txtResult.Text.Length > 0)
            {
                System.Windows.Forms.Clipboard.SetText(this.txtResult.Text);
            }
            else
            {
                MessageBox.Show("No errors");
            }
        }

        private void txtInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;
                Format_SyllableList();
            }
        }

        private void Format_SyllableList()
        {
            var input = this.txtInput.Text;
            var only_errors = this.chkOnlyErrors.Checked;
            var syll_dscrs = GetSyllDescriptions();
            var transitions = GetSyllTransations(syll_dscrs);
            var syll_codes = GetSyllCodes();
            var words = Split_WordList(input);
            var sylls = Split_SyllableList(words, only_errors, transitions, syll_codes);
            this.txtResult.Text = sylls.Format_Table(true);
        }

        private System.Collections.Generic.Dictionary<enrSYLL_STATE, enrSYLL_DSCR> GetSyllDescriptions()
        {
            Mx.bitbase<enrSYLL_DSCR>.populate_bitbase();
            var ret_list = new System.Collections.Generic.Dictionary<enrSYLL_STATE, enrSYLL_DSCR>();
            ret_list.Add(enrSYLL_STATE.handshape_finger, enrSYLL_DSCR.handshape);
            ret_list.Add(enrSYLL_STATE.body_height, enrSYLL_DSCR.location);
            ret_list.Add(enrSYLL_STATE.body_side, enrSYLL_DSCR.location);
            ret_list.Add(enrSYLL_STATE.body_offset_dir, enrSYLL_DSCR.location);
            ret_list.Add(enrSYLL_STATE.body_depth, enrSYLL_DSCR.location);
            ret_list.Add(enrSYLL_STATE.palm_facing, enrSYLL_DSCR.palm_facing);
            ret_list.Add(enrSYLL_STATE.palm_rotate_dir, enrSYLL_DSCR.palm_facing);
            ret_list.Add(enrSYLL_STATE.hand_offset_dir, enrSYLL_DSCR.hand_offset);
            ret_list.Add(enrSYLL_STATE.hand_offset_style, enrSYLL_DSCR.hand_offset);
            ret_list.Add(enrSYLL_STATE.handsyll_separator, enrSYLL_DSCR.syllable_separator);
            ret_list.Add(enrSYLL_STATE.movesyll_separator, enrSYLL_DSCR.syllable_separator);
            ret_list.Add(enrSYLL_STATE.fingerspelling_separator, enrSYLL_DSCR.syllable_separator);
            ret_list.Add(enrSYLL_STATE.word_separator, enrSYLL_DSCR.word_separator);
            ret_list.Add(enrSYLL_STATE.move_depth, enrSYLL_DSCR.movement);
            ret_list.Add(enrSYLL_STATE.move_dir, enrSYLL_DSCR.movement);
            ret_list.Add(enrSYLL_STATE.move_touch_dir, enrSYLL_DSCR.movement);
            ret_list.Add(enrSYLL_STATE.move_touch_style, enrSYLL_DSCR.movement);
            ret_list.Add(enrSYLL_STATE.repeat_flag, enrSYLL_DSCR.repetition);
            ret_list.Add(enrSYLL_STATE.repeat_dir, enrSYLL_DSCR.repetition);
            ret_list.Add(enrSYLL_STATE.basehand_handshape_finger, enrSYLL_DSCR.basehand_handshape);
            ret_list.Add(enrSYLL_STATE.basehand_handshape_thumb, enrSYLL_DSCR.basehand_handshape);
            ret_list.Add(enrSYLL_STATE.basehand_palm_facing, enrSYLL_DSCR.basehand_palm_facing);
            ret_list.Add(enrSYLL_STATE.basehand_palm_pinky, enrSYLL_DSCR.basehand_palm_facing);
            ret_list.Add(enrSYLL_STATE.eyeword_plane, enrSYLL_DSCR.eyeword);
            ret_list.Add(enrSYLL_STATE.eyeword_dir, enrSYLL_DSCR.eyeword);
            ret_list.Add(enrSYLL_STATE.fingerspelling_letter, enrSYLL_DSCR.fingerspelling);
            return ret_list;
        }

        private System.Collections.Generic.List<SyllTransition> GetSyllTransations(System.Collections.Generic.Dictionary<enrSYLL_STATE, enrSYLL_DSCR> ur_dscr_lookup)
        {
            Mx.bitbase<enrSYLL_STATE>.populate_bitbase();
            var ret_list = new System.Collections.Generic.List<SyllTransition>();
            ret_list.Add(new SyllTransition(enrSYLL_STATE.word_separator, enrSYLL_STATE.handshape_thumb, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.word_separator, enrSYLL_STATE.handshape_finger, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.word_separator, enrSYLL_STATE.basehand_flag, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.word_separator, enrSYLL_STATE.eyeword_plane, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.handshape_thumb, enrSYLL_STATE.handshape_finger, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.handshape_inner_thumb, enrSYLL_STATE.handshape_finger, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.handshape_finger, enrSYLL_STATE.handshape_finger, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.handshape_finger, enrSYLL_STATE.handshape_inner_thumb, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.handshape_finger, enrSYLL_STATE.body_height, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.handshape_finger, enrSYLL_STATE.body_side, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.handshape_finger, enrSYLL_STATE.body_depth, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.handshape_finger, enrSYLL_STATE.palm_pinky, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.handshape_finger, enrSYLL_STATE.palm_facing, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.handshape_finger, enrSYLL_STATE.repeat_flag, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.handshape_finger, enrSYLL_STATE.handsyll_separator, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.handshape_finger, enrSYLL_STATE.word_end, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.body_height, enrSYLL_STATE.body_side, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.body_height, enrSYLL_STATE.body_offset_plane, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.body_height, enrSYLL_STATE.body_depth, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.body_height, enrSYLL_STATE.palm_pinky, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.body_height, enrSYLL_STATE.palm_facing, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.body_side, enrSYLL_STATE.body_depth, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.body_side, enrSYLL_STATE.body_offset_plane, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.body_side, enrSYLL_STATE.palm_pinky, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.body_side, enrSYLL_STATE.palm_facing, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.body_offset_plane, enrSYLL_STATE.body_offset_dir, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.body_offset_dir, enrSYLL_STATE.body_offset_dir, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.body_offset_dir, enrSYLL_STATE.body_offset_plane, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.body_offset_dir, enrSYLL_STATE.body_depth, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.body_offset_dir, enrSYLL_STATE.palm_pinky, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.body_offset_dir, enrSYLL_STATE.palm_facing, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.body_depth, enrSYLL_STATE.palm_pinky, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.body_depth, enrSYLL_STATE.palm_facing, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.palm_pinky, enrSYLL_STATE.palm_facing, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.palm_facing, enrSYLL_STATE.palm_rotate_flag, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.palm_facing, enrSYLL_STATE.hand_offset_spacing, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.palm_facing, enrSYLL_STATE.handsyll_separator, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.palm_facing, enrSYLL_STATE.repeat_flag, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.palm_facing, enrSYLL_STATE.word_end, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.palm_rotate_flag, enrSYLL_STATE.palm_rotate_plane, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.palm_rotate_plane, enrSYLL_STATE.palm_rotate_dir, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.palm_rotate_dir, enrSYLL_STATE.hand_offset_spacing, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.palm_rotate_dir, enrSYLL_STATE.repeat_flag, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.palm_rotate_dir, enrSYLL_STATE.handsyll_separator, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.palm_rotate_dir, enrSYLL_STATE.word_end, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.hand_offset_spacing, enrSYLL_STATE.hand_offset_plane, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.hand_offset_plane, enrSYLL_STATE.hand_offset_dir, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.hand_offset_dir, enrSYLL_STATE.hand_offset_style, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.hand_offset_dir, enrSYLL_STATE.handsyll_separator, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.hand_offset_style, enrSYLL_STATE.handsyll_separator, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.handsyll_separator, enrSYLL_STATE.word_separator, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.handsyll_separator, enrSYLL_STATE.move_plane, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.handsyll_separator, enrSYLL_STATE.move_depth, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.handsyll_separator, enrSYLL_STATE.move_touch_flag, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.move_depth, enrSYLL_STATE.repeat_flag, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.move_depth, enrSYLL_STATE.movesyll_separator, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.move_depth, enrSYLL_STATE.word_end, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.move_plane, enrSYLL_STATE.move_plane, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.move_plane, enrSYLL_STATE.move_dir, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.move_dir, enrSYLL_STATE.move_plane, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.move_dir, enrSYLL_STATE.move_dir, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.move_dir, enrSYLL_STATE.repeat_flag, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.move_dir, enrSYLL_STATE.movesyll_separator, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.move_dir, enrSYLL_STATE.move_combine_flag, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.move_dir, enrSYLL_STATE.word_end, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.move_combine_flag, enrSYLL_STATE.move_dir, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.move_touch_flag, enrSYLL_STATE.move_touch_plane, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.move_touch_plane, enrSYLL_STATE.move_touch_dir, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.move_touch_dir, enrSYLL_STATE.move_touch_style, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.move_touch_dir, enrSYLL_STATE.repeat_flag, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.move_touch_dir, enrSYLL_STATE.movesyll_separator, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.move_touch_dir, enrSYLL_STATE.word_end, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.move_touch_style, enrSYLL_STATE.repeat_flag, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.move_touch_style, enrSYLL_STATE.movesyll_separator, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.move_touch_style, enrSYLL_STATE.word_end, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.repeat_flag, enrSYLL_STATE.repeat_plane, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.repeat_flag, enrSYLL_STATE.repeat_dir, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.repeat_flag, enrSYLL_STATE.movesyll_separator, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.repeat_flag, enrSYLL_STATE.word_end, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.repeat_plane, enrSYLL_STATE.repeat_dir, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.repeat_plane, enrSYLL_STATE.word_end, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.repeat_dir, enrSYLL_STATE.repeat_dir, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.repeat_dir, enrSYLL_STATE.repeat_plane, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.repeat_dir, enrSYLL_STATE.movesyll_separator, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.repeat_dir, enrSYLL_STATE.word_end, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.movesyll_separator, enrSYLL_STATE.move_depth, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.movesyll_separator, enrSYLL_STATE.move_plane, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.movesyll_separator, enrSYLL_STATE.move_touch_flag, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.movesyll_separator, enrSYLL_STATE.handshape_thumb, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.movesyll_separator, enrSYLL_STATE.handshape_finger, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.movesyll_separator, enrSYLL_STATE.palm_pinky, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.movesyll_separator, enrSYLL_STATE.palm_facing, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.movesyll_separator, enrSYLL_STATE.word_separator, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.basehand_flag, enrSYLL_STATE.basehand_handshape_finger, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.basehand_flag, enrSYLL_STATE.basehand_palm_facing, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.basehand_handshape_finger, enrSYLL_STATE.basehand_handshape_finger, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.basehand_handshape_finger, enrSYLL_STATE.basehand_handshape_thumb, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.basehand_handshape_finger, enrSYLL_STATE.basehand_palm_facing, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.basehand_handshape_thumb, enrSYLL_STATE.basehand_handshape_finger, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.basehand_handshape_thumb, enrSYLL_STATE.basehand_palm_facing, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.basehand_handshape_thumb, enrSYLL_STATE.movesyll_separator, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.basehand_palm_facing, enrSYLL_STATE.basehand_palm_pinky, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.basehand_palm_facing, enrSYLL_STATE.movesyll_separator, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.basehand_palm_pinky, enrSYLL_STATE.movesyll_separator, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.eyeword_plane, enrSYLL_STATE.eyeword_dir, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.eyeword_plane, enrSYLL_STATE.eyeword_separator, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.eyeword_plane, enrSYLL_STATE.fingerspelling_separator, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.eyeword_plane, enrSYLL_STATE.word_end, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.eyeword_dir, enrSYLL_STATE.eyeword_separator, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.eyeword_dir, enrSYLL_STATE.eyeword_dir, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.eyeword_dir, enrSYLL_STATE.word_end, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.eyeword_separator, enrSYLL_STATE.eyeword_plane, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.eyeword_separator, enrSYLL_STATE.eyeword_dir, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.fingerspelling_separator, enrSYLL_STATE.fingerspelling_letter, enrSYLL_DSCR.none));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.fingerspelling_letter, enrSYLL_STATE.fingerspelling_letter, enrSYLL_DSCR.incomplete));
            ret_list.Add(new SyllTransition(enrSYLL_STATE.fingerspelling_letter, enrSYLL_STATE.word_end, enrSYLL_DSCR.none));

            foreach (var entry in ret_list)
            {
                if (entry.new_syll == enrSYLL_DSCR.none)
                {
                    if (ur_dscr_lookup.ContainsKey(entry.cur_state))
                    {
                        entry.new_syll = ur_dscr_lookup[entry.cur_state];
                    }
                    else
                    {
                        //var abc = "";
                    }
                }
            }

            return ret_list;
        }

        private System.Collections.Generic.List<SyllCode> GetSyllCodes()
        {
            Mx.bitbase<enrSYLL_STATE>.populate_bitbase();
            var ret_list = new System.Collections.Generic.List<SyllCode>();
            ret_list.Add(new SyllCode(enrSYLL_STATE.handshape_thumb, 'c'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.handshape_thumb, 'j'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.handshape_thumb, 'r'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.handshape_thumb, 'y'));
            CopySyllCode(enrSYLL_STATE.handshape_thumb, enrSYLL_STATE.handshape_inner_thumb, ret_list);
            CopySyllCode(enrSYLL_STATE.handshape_thumb, enrSYLL_STATE.basehand_handshape_thumb, ret_list);

            ret_list.Add(new SyllCode(enrSYLL_STATE.handshape_finger, 'f'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.handshape_finger, 'l'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.handshape_finger, 'n'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.handshape_finger, 't'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.handshape_finger, 'u'));
            CopySyllCode(enrSYLL_STATE.handshape_finger, enrSYLL_STATE.basehand_handshape_finger, ret_list);

            ret_list.Add(new SyllCode(enrSYLL_STATE.body_height, 'W'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.body_height, 'C'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.body_height, 'D'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.body_height, 'Y'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.body_side, 'K'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.body_depth, 'D'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.palm_pinky, 'i'));
            CopySyllCode(enrSYLL_STATE.palm_pinky, enrSYLL_STATE.basehand_palm_pinky, ret_list);

            ret_list.Add(new SyllCode(enrSYLL_STATE.palm_facing, 'k'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.palm_facing, 'M'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.palm_facing, 'U'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.basehand_palm_facing, 'y'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.basehand_palm_facing, 'M'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.basehand_palm_facing, 'U'));

            ret_list.Add(new SyllCode(enrSYLL_STATE.palm_rotate_flag, 'o'));
            CopySyllCode(enrSYLL_STATE.palm_rotate_flag, enrSYLL_STATE.basehand_palm_rotate_flag, ret_list);

            ret_list.Add(new SyllCode(enrSYLL_STATE.palm_rotate_plane, 'h'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.palm_rotate_plane, 'q'));
            CopySyllCode(enrSYLL_STATE.palm_rotate_plane, enrSYLL_STATE.hand_offset_plane, ret_list);
            CopySyllCode(enrSYLL_STATE.palm_rotate_plane, enrSYLL_STATE.body_offset_plane, ret_list);
            CopySyllCode(enrSYLL_STATE.palm_rotate_plane, enrSYLL_STATE.basehand_palm_rotate_plane, ret_list);

            ret_list.Add(new SyllCode(enrSYLL_STATE.palm_rotate_dir, 'k'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.palm_rotate_dir, 'y'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.palm_rotate_dir, 'v'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.palm_rotate_dir, '^'));
            CopySyllCode(enrSYLL_STATE.palm_rotate_dir, enrSYLL_STATE.body_offset_dir, ret_list);
            CopySyllCode(enrSYLL_STATE.palm_rotate_dir, enrSYLL_STATE.hand_offset_dir, ret_list);
            CopySyllCode(enrSYLL_STATE.palm_rotate_dir, enrSYLL_STATE.move_touch_dir, ret_list);
            CopySyllCode(enrSYLL_STATE.palm_rotate_dir, enrSYLL_STATE.repeat_dir, ret_list);
            CopySyllCode(enrSYLL_STATE.palm_rotate_dir, enrSYLL_STATE.basehand_palm_rotate_dir, ret_list);

            ret_list.Add(new SyllCode(enrSYLL_STATE.hand_offset_spacing, 'T'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.hand_offset_spacing, 'H'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.hand_offset_style, 'c'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.hand_offset_style, 'x'));
            CopySyllCode(enrSYLL_STATE.hand_offset_style, enrSYLL_STATE.move_touch_style, ret_list);

            ret_list.Add(new SyllCode(enrSYLL_STATE.handsyll_separator, '-'));
            CopySyllCode(enrSYLL_STATE.handsyll_separator, enrSYLL_STATE.movesyll_separator, ret_list);
            CopySyllCode(enrSYLL_STATE.handsyll_separator, enrSYLL_STATE.word_separator, ret_list);
            CopySyllCode(enrSYLL_STATE.handsyll_separator, enrSYLL_STATE.eyeword_separator, ret_list);

            ret_list.Add(new SyllCode(enrSYLL_STATE.move_depth, 'D'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.move_plane, 'p'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.move_plane, 'q'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.move_plane, 'd'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.move_plane, 'h'));
            CopySyllCode(enrSYLL_STATE.move_plane, enrSYLL_STATE.move_touch_plane, ret_list);
            CopySyllCode(enrSYLL_STATE.move_plane, enrSYLL_STATE.repeat_plane, ret_list);
            CopySyllCode(enrSYLL_STATE.move_plane, enrSYLL_STATE.eyeword_plane, ret_list);

            ret_list.Add(new SyllCode(enrSYLL_STATE.move_dir, 'e'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.move_dir, 'o'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.move_dir, 'k'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.move_dir, 'y'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.move_dir, 'v'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.move_dir, '^'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.move_dir, '~'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.move_dir, 'D'));

            ret_list.Add(new SyllCode(enrSYLL_STATE.move_touch_flag, 'T'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.move_combine_flag, 'r'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.repeat_flag, 'z'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.basehand_flag, 'p'));

            ret_list.Add(new SyllCode(enrSYLL_STATE.eyeword_dir, 'e'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.eyeword_dir, 'o'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.eyeword_dir, 'k'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.eyeword_dir, 'y'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.eyeword_dir, 'v'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.eyeword_dir, '^'));

            ret_list.Add(new SyllCode(enrSYLL_STATE.fingerspelling_separator, '~'));
            ret_list.Add(new SyllCode(enrSYLL_STATE.fingerspelling_letter, '*'));

            return ret_list;
        }

        private void CopySyllCode(enrSYLL_STATE ur_cur_state, enrSYLL_STATE ur_new_state, System.Collections.Generic.List<SyllCode> ur_syll_codes)
        {
            var append_list = new System.Collections.Generic.List<SyllCode>();
            foreach (var entry in ur_syll_codes)
            {
                if (entry.cur_state == ur_cur_state)
                {
                    append_list.Add(new SyllCode(ur_new_state, entry.syll_char));
                }
            }

            foreach (var entry in append_list)
            {
                ur_syll_codes.Add(entry);
            }
        }

        private Mx.bitbase<enrWORD>.table_row Split_WordList(string ur_input)
        {
            var ret_words = new Mx.bitbase<enrWORD>.table_row();
            if (ur_input.Length == 0)
            {
                ret_words.Persist_Read(@"C:\Users\Dad\Downloads\2021m10d31 ASLSJ Parse\SampleData.tsv");
                foreach (var row in ret_words)
                {
                    row[enrWORD.word] = row[enrWORD.word].Replace(char.ConvertFromUtf32(8211).ToString(), "--").Trim();
                }
            }
            else
            {
                foreach (var entry in ur_input.Split('\n'))
                {
                    if (string.IsNullOrWhiteSpace(entry) == false)
                    {
                        ret_words.add_row()[enrWORD.word] = entry.Replace(char.ConvertFromUtf32(8211).ToString(), "--").Trim();
                    }
                }
            }

            return ret_words;
        }

        private Mx.bitbase<enrSYLL>.table_row Split_SyllableList(
            Mx.bitbase<enrWORD>.table_row ur_words,
            bool ur_flag_only_errors,
            System.Collections.Generic.List<SyllTransition> ur_transitions,
            System.Collections.Generic.List<SyllCode> ur_syll_codes)
        {
            Mx.bitbase<enrSYLL_STATE>.populate_bitbase();
            var ret_syll = new Mx.bitbase<enrSYLL>.table_row();
            foreach (var word_rec in ur_words)
            {
                var word_syll = new Mx.bitbase<enrSYLL>.table_row();
                var cur_state = enrSYLL_STATE.word_separator;
                var stp_syll = new System.Text.StringBuilder();
                var SYLSEQ = 0;
                var flag_error = false;
                foreach (var entry in word_rec[enrWORD.word])
                {
                    var entry_lc = entry;
                    if (cur_state == enrSYLL_STATE.word_separator)
                    {
                        entry_lc = entry.ToString().ToLower()[0];
                    }

                    var found_transition = false;
                    foreach (var transition_rec in ur_transitions)
                    {
                        if (cur_state == transition_rec.cur_state)
                        {
                            found_transition = Append_Syll(
                                entry_lc,
                                transition_rec.match_state,
                                transition_rec.new_syll,
                                ref cur_state,
                                ref SYLSEQ,
                                stp_syll,
                                word_syll,
                                word_rec,
                                ur_syll_codes);

                            if (found_transition)
                            {
                                break;
                            }
                        }
                    }

                    if (found_transition == false)
                    {
                        stp_syll.Append($" - Unknown {cur_state.pname} char: ").Append(entry_lc);
                        flag_error = true;
                        break;
                    }
                }

                {
                    var entry_lc = '-';
                    var found_transition = false;
                    foreach (var transition_rec in ur_transitions)
                    {
                        if (cur_state == transition_rec.cur_state && transition_rec.match_state == enrSYLL_STATE.word_end)
                        {
                            found_transition = Append_Syll(
                                entry_lc,
                                transition_rec.match_state,
                                transition_rec.new_syll,
                                ref cur_state,
                                ref SYLSEQ,
                                stp_syll,
                                word_syll,
                                word_rec,
                                ur_syll_codes);

                            if (found_transition)
                            {
                                break;
                            }
                        }
                    }

                    if (found_transition == false)
                    {
                        stp_syll.Append($" - Unknown {cur_state.pname} char: ").Append(entry_lc);
                        Append_Syll(
                            entry_lc,
                            enrSYLL_STATE.word_end,
                            enrSYLL_DSCR.none,
                            ref cur_state,
                            ref SYLSEQ,
                            stp_syll,
                            word_syll,
                            word_rec,
                            ur_syll_codes);
                        flag_error = true;
                    }
                }

                if (flag_error == true || ur_flag_only_errors == false)
                {
                    foreach (var entry in word_syll)
                    {
                        ret_syll.Add(entry);
                    }
                }
            }

            return ret_syll;
        }

        void Append_Syll(
            ref int ref_syll_seq,
            System.Text.StringBuilder ur_stp_syll,
            enrSYLL_DSCR ur_syll_dscr,
            Mx.bitbase<enrSYLL>.table_row ur_sylls,
            Mx.bitbase<enrWORD>.row_enum ur_word_rec)
        {
            if (ur_stp_syll.Length > 0)
            {
                var last_syll = ur_sylls.add_row();
                ref_syll_seq += 1;
                last_syll[enrSYLL.word] = ur_word_rec[enrWORD.word];
                last_syll[enrSYLL.syll_seq] = ref_syll_seq.ToString();
                last_syll[enrSYLL.syll_text] = ur_stp_syll.ToString();
                last_syll[enrSYLL.syll_dscr] = ur_syll_dscr.pname;
                ur_stp_syll.Clear();
            }
        }

        bool Append_Syll(
            char ur_entry,
            enrSYLL_STATE ur_test_syll,
            enrSYLL_DSCR flag_new_syll,
            ref enrSYLL_STATE ref_cur_state,
            ref int ref_syll_seq,
            System.Text.StringBuilder ur_stp_syll,
            Mx.bitbase<enrSYLL>.table_row ur_sylls,
            Mx.bitbase<enrWORD>.row_enum ur_word_rec,
            System.Collections.Generic.List<SyllCode> ur_syll_codes)
        {
            var ret_is_syll = false;
            foreach (var entry in ur_syll_codes)
            {
                if (ur_test_syll == entry.cur_state)
                {
                    if (ur_entry == entry.syll_char || entry.syll_char == '*')
                    {
                        ret_is_syll = true;
                    }
                }
            }

            if (ret_is_syll || ur_test_syll == enrSYLL_STATE.word_end)
            {
                if (flag_new_syll != enrSYLL_DSCR.incomplete)
                {
                    ret_is_syll = true;
                    Append_Syll(ref ref_syll_seq, ur_stp_syll, flag_new_syll, ur_sylls, ur_word_rec);
                }

                ref_cur_state = ur_test_syll;
                ur_stp_syll.Append(ur_entry);
            }

            return ret_is_syll;
        }

        class enrWORD : Mx.bitbase
        {
            public static enrWORD word { get; set; } = new enrWORD();
        }
        class enrSYLL : Mx.bitbase
        {
            public static enrSYLL word { get; set; } = new enrSYLL();
            public static enrSYLL syll_seq { get; set; } = new enrSYLL();
            public static enrSYLL syll_text { get; set; } = new enrSYLL();
            public static enrSYLL syll_dscr { get; set; } = new enrSYLL();
        }
        class enrSYLL_STATE : Mx.bitbase
        {
            public static enrSYLL_STATE handshape_thumb { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE handshape_inner_thumb { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE handshape_finger { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE body_height { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE body_side { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE body_offset_plane { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE body_offset_dir { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE body_depth { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE palm_pinky { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE palm_facing { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE palm_rotate_flag { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE palm_rotate_plane { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE palm_rotate_dir { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE hand_offset_spacing { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE hand_offset_plane { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE hand_offset_dir { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE hand_offset_style { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE handsyll_separator { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE movesyll_separator { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE word_separator { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE move_depth { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE move_plane { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE move_dir { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE move_combine_flag { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE move_touch_flag { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE move_touch_plane { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE move_touch_dir { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE move_touch_style { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE repeat_flag { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE repeat_plane { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE repeat_dir { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE basehand_flag { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE basehand_handshape_thumb { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE basehand_handshape_finger { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE basehand_palm_pinky { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE basehand_palm_facing { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE basehand_palm_rotate_flag { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE basehand_palm_rotate_plane { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE basehand_palm_rotate_dir { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE eyeword_plane { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE eyeword_dir { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE eyeword_separator { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE fingerspelling_separator { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE fingerspelling_letter { get; set; } = new enrSYLL_STATE();
            public static enrSYLL_STATE word_end { get; set; } = new enrSYLL_STATE();
        }
        class enrSYLL_DSCR : Mx.bitbase
        {
            public static enrSYLL_DSCR none { get; set; } = new enrSYLL_DSCR();
            public static enrSYLL_DSCR incomplete { get; set; } = new enrSYLL_DSCR();
            public static enrSYLL_DSCR handshape { get; set; } = new enrSYLL_DSCR();
            public static enrSYLL_DSCR location { get; set; } = new enrSYLL_DSCR();
            public static enrSYLL_DSCR palm_facing { get; set; } = new enrSYLL_DSCR();
            public static enrSYLL_DSCR hand_offset { get; set; } = new enrSYLL_DSCR();
            public static enrSYLL_DSCR syllable_separator { get; set; } = new enrSYLL_DSCR();
            public static enrSYLL_DSCR word_separator { get; set; } = new enrSYLL_DSCR();
            public static enrSYLL_DSCR movement { get; set; } = new enrSYLL_DSCR();
            public static enrSYLL_DSCR repetition { get; set; } = new enrSYLL_DSCR();
            public static enrSYLL_DSCR basehand_handshape { get; set; } = new enrSYLL_DSCR();
            public static enrSYLL_DSCR basehand_palm_facing { get; set; } = new enrSYLL_DSCR();
            public static enrSYLL_DSCR eyeword { get; set; } = new enrSYLL_DSCR();
            public static enrSYLL_DSCR fingerspelling { get; set; } = new enrSYLL_DSCR();
            public static enrSYLL_DSCR word_end { get; set; } = new enrSYLL_DSCR();
        }

        class SyllCode
        {
            public enrSYLL_STATE cur_state;
            public char syll_char;
            public SyllCode(
                enrSYLL_STATE ur_cur_state,
                char ur_syll_char)
            {
                this.cur_state = ur_cur_state;
                this.syll_char = ur_syll_char;
            }
        }

        class SyllTransition
        {
            public enrSYLL_STATE cur_state;
            public enrSYLL_STATE match_state;
            public enrSYLL_DSCR new_syll;
            public SyllTransition(
                enrSYLL_STATE ur_cur_state,
                enrSYLL_STATE ur_match_state,
                enrSYLL_DSCR ur_new_syll)
            {
                this.cur_state = ur_cur_state;
                this.match_state = ur_match_state;
                this.new_syll = ur_new_syll;
            }
        }

    }
}

using ASPxGridViewAIIntegration.Models;
using Azure.AI.OpenAI;
using DevExpress.AIIntegration;
using DevExpress.AIIntegration.OpenAI;
using DevExpress.Web;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPxGridViewAIIntegration
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private static List<DictionaryEntry> _originalEntries;
        private SmartFilterProvider _filterProvider;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (_originalEntries == null) {
                _originalEntries = GenerateData();
            }

            ASPxGridView1.DataSource = _originalEntries;
            if (!IsPostBack) { 
                ASPxGridView1.DataBind();
            }

            var embeddingGenerator = (IEmbeddingGenerator<string, Embedding<float>>)Application["EmbeddingGenerator"];

            _filterProvider = new SmartFilterProvider(embeddingGenerator);
        }

        protected async void CallbackPanel_Callback(object sender, CallbackEventArgsBase e)
        {
            var payload = JsonSerializer.Deserialize<CallbackPayload>(e.Parameter);
            var searchText = (payload?.search ?? string.Empty).Trim();
            var similarity = payload?.similarity;

            if (string.IsNullOrEmpty(searchText))
            {
                ASPxGridView1.DataSource = _originalEntries;
                ASPxGridView1.DataBind();
                return;
            }

            var descriptions = _originalEntries.Select(x => $"{x.Name} - {x.Description}").ToList();
            descriptions.Add(searchText);

            try
            {
                await _filterProvider.FillCacheAsync(descriptions);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }

            var ranked = _originalEntries
                .Select(item => new
                {
                    Item = item,
                    Similarity = _filterProvider.GetSimilarity($"{item.Name} - {item.Description}", searchText)
                })
                .OrderByDescending(x => x.Similarity)
                .Where(x => x.Similarity > similarity)
                .Select(x => x.Item)
                .ToList();

            ASPxGridView1.DataSource = ranked;
            ASPxGridView1.DataBind();
        }

        class CallbackPayload { public string search { get; set; } public float similarity { get; set; } }

        public List<DictionaryEntry> GenerateData()
        {
            return new List<DictionaryEntry>
            {
                new DictionaryEntry(1, "Car", "A vehicle with four wheels"),
                new DictionaryEntry(2, "Bus", "A large vehicle for transporting passengers"),
                new DictionaryEntry(3, "Tea", "A hot drink made from brewed leaves"),
                new DictionaryEntry(4, "Coffee", "A hot drink made from brewed beans"),
                new DictionaryEntry(5, "Computer", "An electronic device for processing data"),
                new DictionaryEntry(6, "Mouse", "An input device for controlling the cursor on the screen"),
                new DictionaryEntry(7, "Airplane", "An aircraft for transporting passengers and cargo"),
                new DictionaryEntry(8, "Table", "Furniture for working or dining"),
                new DictionaryEntry(9, "Chair", "A comfortable seat with armrests"),
                new DictionaryEntry(10, "Phone", "A device for voice and text communication"),
                new DictionaryEntry(11, "Book", "A printed publication with texts and illustrations"),
                new DictionaryEntry(12, "Pen", "A tool for writing with ink"),
                new DictionaryEntry(13, "Watch", "A device for measuring time"),
                new DictionaryEntry(14, "Television", "A device for watching videos and broadcasts"),
                new DictionaryEntry(15, "Camera", "A device for taking photographs"),
                new DictionaryEntry(16, "Bicycle", "A two-wheeled vehicle"),
                new DictionaryEntry(17, "Plate", "A dish for serving food"),
                new DictionaryEntry(18, "Cup", "A container for drinking hot beverages"),
                new DictionaryEntry(19, "Backpack", "A bag carried on the back"),
                new DictionaryEntry(20, "Calculator", "An electronic device for calculations"),
                new DictionaryEntry(21, "Tablet", "A mobile device with a touch screen"),
                new DictionaryEntry(22, "Keyboard", "An input device for typing text"),
                new DictionaryEntry(23, "Spoon", "A utensil for eating soups and cereals"),
                new DictionaryEntry(24, "Fork", "A utensil for picking up food"),
                new DictionaryEntry(25, "Knife", "A tool for cutting food"),
                new DictionaryEntry(26, "Lamp", "A lighting device"),
                new DictionaryEntry(27, "Wardrobe", "Furniture for storing clothes and things"),
                new DictionaryEntry(28, "Mirror", "An object for reflecting images"),
                new DictionaryEntry(29, "Microwave", "A device for quickly heating food"),
                new DictionaryEntry(30, "Refrigerator", "A device for keeping food at low temperatures"),
                new DictionaryEntry(31, "Vacuum Cleaner", "A device for cleaning dust and debris"),
                new DictionaryEntry(32, "Kettle", "A container for boiling water"),
                new DictionaryEntry(33, "Bottle", "A container for storing liquids"),
                new DictionaryEntry(34, "Glasses", "A device for correcting vision"),
                new DictionaryEntry(35, "Headphones", "A device for personal audio listening"),
                new DictionaryEntry(36, "Speaker", "A device for playing sound"),
                new DictionaryEntry(37, "Microphone", "A device for recording and transmitting sound"),
                new DictionaryEntry(38, "Mouse", "A small rodent"),
                new DictionaryEntry(39, "Power Bank", "A device for charging gadgets on the go"),
                new DictionaryEntry(40, "Monitor", "A screen for displaying information from a computer"),
                new DictionaryEntry(41, "Notebook", "A book of blank pages for writing notes"),
                new DictionaryEntry(42, "Pencil", "A tool for writing or drawing with graphite"),
                new DictionaryEntry(43, "Backpack", "A bag worn on the back for carrying items"),
                new DictionaryEntry(44, "Shoes", "Footwear for protection and comfort"),
                new DictionaryEntry(45, "Socks", "Garments worn on the feet"),
                new DictionaryEntry(46, "T-shirt", "A short-sleeved casual top"),
                new DictionaryEntry(47, "Jacket", "A garment for the upper body"),
                new DictionaryEntry(48, "Hat", "A head covering for warmth or fashion"),
                new DictionaryEntry(49, "Scarf", "A cloth worn around the neck"),
                new DictionaryEntry(50, "Gloves", "Hand coverings for warmth or protection"),
                new DictionaryEntry(51, "Umbrella", "A device for protection against rain or sun"),
                new DictionaryEntry(52, "Wallet", "A small case for holding money and personal items"),
                new DictionaryEntry(53, "Purse", "A handbag used to carry everyday items"),
                new DictionaryEntry(54, "Watch", "A timepiece worn on the wrist"),
                new DictionaryEntry(55, "Belt", "A strap worn around the waist"),
                new DictionaryEntry(56, "Ring", "A circular band worn on the finger"),
                new DictionaryEntry(57, "Necklace", "A piece of jewelry worn around the neck"),
                new DictionaryEntry(58, "Bracelet", "A piece of jewelry worn around the wrist"),
                new DictionaryEntry(59, "Earrings", "Jewelry worn on the earlobes"),
                new DictionaryEntry(60, "Sunglasses", "Eyewear for protection against sunlight"),
                new DictionaryEntry(61, "Notebook", "A portable computer"),
                new DictionaryEntry(62, "Printer", "A device that produces paper copies"),
                new DictionaryEntry(63, "Router", "A device routing network packets"),
                new DictionaryEntry(64, "Modem", "A device for internet access"),
                new DictionaryEntry(65, "Smartphone", "A mobile phone with advanced features"),
                new DictionaryEntry(66, "Blender", "An appliance for mixing food"),
                new DictionaryEntry(67, "Toaster", "A device for browning bread"),
                new DictionaryEntry(68, "Oven", "An appliance for baking"),
                new DictionaryEntry(69, "Stove", "An appliance for cooking food"),
                new DictionaryEntry(70, "Dishwasher", "An appliance for cleaning dishes"),
                new DictionaryEntry(71, "Washing Machine", "An appliance for washing clothes"),
                new DictionaryEntry(72, "Dryer", "An appliance for drying clothes"),
                new DictionaryEntry(73, "Iron", "A device for smoothing fabric"),
                new DictionaryEntry(74, "Ironing Board", "A surface for ironing clothes"),
                new DictionaryEntry(75, "Broom", "A tool for sweeping"),
                new DictionaryEntry(76, "Dustpan", "A scoop for collecting dust"),
                new DictionaryEntry(77, "Mop", "A tool for cleaning floors"),
                new DictionaryEntry(78, "Bucket", "A container for carrying liquids"),
                new DictionaryEntry(79, "Detergent", "A cleaning agent"),
                new DictionaryEntry(80, "Sponge", "A porous cleaning material"),
                new DictionaryEntry(81, "Towel", "A cloth for drying"),
                new DictionaryEntry(82, "Toothbrush", "A brush for cleaning teeth"),
                new DictionaryEntry(83, "Toothpaste", "A paste for cleaning teeth"),
                new DictionaryEntry(84, "Shampoo", "A liquid for washing hair"),
                new DictionaryEntry(85, "Conditioner", "A product for softening hair"),
                new DictionaryEntry(86, "Soap", "A substance for washing"),
                new DictionaryEntry(87, "Body Wash", "A liquid soap"),
                new DictionaryEntry(88, "Lotion", "A cream for moisturizing skin"),
                new DictionaryEntry(89, "Deodorant", "A substance for preventing body odor"),
                new DictionaryEntry(90, "Perfume", "A fragrant liquid"),
                new DictionaryEntry(91, "Razor", "A tool for shaving hair"),
                new DictionaryEntry(92, "Shaving Cream", "A cream used before shaving"),
                new DictionaryEntry(93, "Hair Dryer", "A device for drying hair"),
                new DictionaryEntry(94, "Comb", "A tool for arranging hair"),
                new DictionaryEntry(95, "Hairbrush", "A brush for styling hair"),
                new DictionaryEntry(96, "Nail Clippers", "A tool for trimming nails"),
                new DictionaryEntry(97, "Nail File", "A tool for shaping nails"),
                new DictionaryEntry(98, "Tweezers", "A tool for plucking hair"),
                new DictionaryEntry(99, "Scissors", "A tool for cutting"),
                new DictionaryEntry(100, "Band-Aid", "A small adhesive bandage")
            };
        }

    }
}
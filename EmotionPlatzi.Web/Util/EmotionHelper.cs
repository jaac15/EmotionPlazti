using EmotionPlatzi.Web.Models;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;

namespace EmotionPlatzi.Web.Util
{
    public class EmotionHelper
    {
        public EmotionServiceClient EmoClient ;
        public EmotionHelper(string key)
        {
            EmoClient = new EmotionServiceClient(key);

        }

        public async Task<EmoPicture> DetectAndExtractFacesAsync(Stream ImageStream)
        {
            Emotion[] emotions = await EmoClient.RecognizeAsync(ImageStream);
            var emopic = new EmoPicture();
            emopic.Faces = ExtractFaces(emotions,emopic);
            return emopic;

        }

        private ObservableCollection<EmoFace> ExtractFaces(Emotion[] emotions, EmoPicture emopic)
        {
            var Faces = new ObservableCollection<EmoFace>();
          
            foreach (var emotion in emotions)
            {
                var emoface =new EmoFace() { X = emotion.FaceRectangle.Left,
                                             Y = emotion.FaceRectangle.Top,
                                             Height =  emotion.FaceRectangle.Height,
                                             Width = emotion.FaceRectangle.Width,
                                             Picture = emopic,
                                             };
                emoface.Emotion = ProcessEmotion(emotion.Scores,emoface);
                Faces.Add(emoface);
            }
            return Faces;
        }

        private ObservableCollection<EmoEmotion> ProcessEmotion(Scores scores, EmoFace face)
        {
            var emotionlist = new ObservableCollection<EmoEmotion>();
            var properties = scores.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var filterProperties = properties.Where(p => p.PropertyType == typeof(float));
            EmoEmotionEnum emotype = EmoEmotionEnum.undetermined;
            foreach (var property in filterProperties)
            {
                if (!Enum.TryParse<EmoEmotionEnum>(property.Name, out emotype))
                    emotype = EmoEmotionEnum.undetermined;
                var emotion = new EmoEmotion();
                emotion.Score = (float)property.GetValue(scores);
                emotion.EmotionType = emotype;
                emotion.Face = face;
                emotionlist.Add(emotion);

            }
            return emotionlist;
        }
    }

   
}
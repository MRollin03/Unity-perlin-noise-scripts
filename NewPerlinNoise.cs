using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewPerlinNoise : MonoBehaviour
{
  private int[] perm = new int[512]; // opsætter et overflow int array, for vores hashtabel senere i koden.
  
  //UI elementer
  public Slider GridScaleSlider, SEEDSlider, ReselusitionSlider;
  public TMP_Text SEEDtext;
         Renderer render;
         Texture2D texture;

  // Billed egenskaber 
  [Range(1,255)] // Range er en unity editor atribute brugt til at 
                    // limitere værdier i vores editor
  public int height, width; // opløsning i pixels


  [Range(1,100)]    // ærklere vores perlingrid værdier.
  public float GridScale, offsetX, offsetY; 
  public float SEED, offsetYtime, offsetXtime;

    private void Start() {
        initPerlinNoise(); // opsætter vores perlin noise hashtable.
                            // og hashoverflow table.

        render = GetComponent<Renderer>(); // Henter vores objects Render component.
    }

    private void Update() {
        //  opdater vores værdier der ændre vores texture propeties til vores 
        //  perlin algorytme og UI elementer.

        GridScale = System.Convert.ToInt16(GridScaleSlider.value);
        SEED = SEEDSlider.value;
        height = System.Convert.ToInt16(ReselusitionSlider.value);
        width = System.Convert.ToInt16(ReselusitionSlider.value);
        SEEDtext.text = System.Convert.ToString(SEED);
    }

    public void NewTexture(){ 
        render.material.mainTexture = GenerateTexture(); // kalder funktion
                                                         // sætter den nye generet tezxture
                                                         // til main texture af vores rendere.
                                                         // component.
    }

    Texture2D GenerateTexture(){

        texture = new Texture2D(width, height); // vi ærklere ny texture2D texture.

        for (int x = 0; x < width; x++) // et forloop der har styr på vores x 
                                        // textures x kordinator 
        {
            for (int y = 0; y < height; y++)// et forloop der har styr på vores x 
            {                                // textures Y kordinator 

                Color color = (CalculateColor(x,y)); // Kalder funktion som beregner vores 
                                                    // nye farve for kordinatet

                texture.SetPixel(x,y,color); // Sætter vores pixels farve til den beregner 
                                            // farve. fra sidste linje
                
            }
        }
        texture.Apply();    // opdater vores textures pixels
        return texture; //retunere vores texture.
    }


Color CalculateColor (int x, int y){
    float xCoord = (float)x / height * GridScale + SEED; // Beregner vores pixels x og y kordinat
    float yCoord = (float)y / width * GridScale + SEED; // i forhold til vores perlin grid.

    float sample = Pnoise((float) xCoord, (float) yCoord, 1); // kalder vores pelrin algorhytme

    return new Color(sample, sample, sample); //retunere vores nye RGB() color object.
}





// Perlin noise orginal kode kilde oversat fra C. :http://riven8192.blogspot.com/2010/08/calculate-perlinnoise-twice-as-fast.html.
//  Koden er en optimeret version af Ken Perlin's fra 2002, dog har præsis samme princip.

   void initPerlinNoise() // Denne funktion opstil elr vores hashtabel inden brug.
   {
      int[]  permutation = { // vi ærklæere et inteture array bestående af 255 tal
                             // en del af ken perlins implementation.
         151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7, 225, 140, 36, 103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23, 190, 6, 148, 247, 120, 234, 75, 0, 26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32, 57, 177, 33, 88, 237, 149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175, 74, 165, 71, 134, 139, 48, 27, 166, 77, 146, 158, 231, 83, 111, 229, 122, 60, 211, 133, 230, 220, 105, 92, 41, 55, 46, 245, 40, 244, 102, 143, 54, 65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132, 187, 208, 89, 18, 169, 200, 196, 135, 130, 116, 188, 159, 86, 164, 100, 109, 198, 173, 186, 3, 64, 52, 217, 226, 250, 124, 123, 5, 202, 38, 147, 118, 126, 255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189, 28, 42, 223, 183, 170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221, 153, 101, 155, 167, 43, 172, 9, 129, 22, 39, 253, 19, 98, 108, 110, 79, 113, 224, 232, 178, 185, 112, 104, 218, 246, 97, 228, 251, 34, 242, 193, 238, 210, 144, 12, 191, 179, 162, 241, 81, 51, 145, 235, 249,
         14, 239, 107, 49, 192, 214, 31, 181, 199, 106, 157, 184, 84, 204, 176, 115, 121, 50, 45, 127, 4, 150, 254, 138, 236, 205, 93, 222, 114, 67, 29, 24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180 };

      for (int i = 0; i < 256; i++) //her tildeler vi vores overflow int array med værdierne fra vroes orginale hashtable.
         perm[256 + i] = perm[i] = permutation[i]; //dublikere sådan set bare hashtablen dobbelt i vores overflow array.
   }

    float  Pnoise(float x, float y, float z) // Pnoise er funktionen vi kan kalde fra de andre scripts.
                                            // funktionen kan bruges til alle de forbrugs sager der skal bruge.
                                            // mindre en 4-Dimensioner, Dvs 1, 2 og 3- dimensionelle brug.
   {
    // Vi ærklere en masse værdier.
      int ix,iy,iz,gx,gy,gz;
      int a0,b0,aa,ab,ba,bb;

      int aa0,ab0,ba0,bb0;
      int aa1,ab1,ba1,bb1;
      float a1,a2,a3,a4,a5,a6,a7,a8;
      float u,v,w,a8_5,a4_1;

      ix = (int)x; x-=ix;   //  Finder vores gitter nummer.
      iy = (int)y; y-=iy;   // Der hvor vores kordinat befidner sig.
      iz = (int)z; z-=iz;

      gx = ix & 0xFF;       //  Omdanner vores int til 8 bit
      gy = iy & 0xFF;       // f.eks ( 11 & 11111111 = 00001011 ).
      gz = iz & 0xFF;
      
      a0 = gy+perm[gx];     // finder værdier af nogle dele af hjørnerne.
      b0 = gy+perm[gx + 1]; // bruges i de ænderlige hjørne værdier neden under.
      aa = gz+perm[a0];
      ab = gz+perm[a0 + 1];
      ba = gz+perm[b0];
      bb = gz+perm[b0 + 1];

      aa0 = perm[aa]; aa1 = perm[aa + 1];   // henter vores værdier af alle vores 8    
      ab0 = perm[ab]; ab1 = perm[ab + 1];   // hjørner af vores 3-Dimensionelle Cube.
      ba0 = perm[ba]; ba1 = perm[ba + 1];   // fra Vores hashtabel.
      bb0 = perm[bb]; bb1 = perm[bb + 1];

      a1 = grad(bb1, x-1, y-1, z-1);    // Grad() Funktionen står for skalarproduktet.
      a2 = grad(ab1, x  , y-1, z-1);    // Mellem gradient vektor og distance vektor
      a3 = grad(ba1, x-1, y  , z-1);    // grad() er en funktion der bliver kaldt.  
      a4 = grad(aa1, x  , y  , z-1);    // (findes længere nede).
      a5 = grad(bb0, x-1, y-1, z  );
      a6 = grad(ab0, x  , y-1, z  );
      a7 = grad(ba0, x-1, y  , z  );
      a8 = grad(aa0, x  , y  , z  );

      u = fade(x);  // kalder vores fade funktion med vores.
      v = fade(y);  // punkts lokale kordinator for gitteret.
      w = fade(z);  // manipulere vores kordinator.

      a8_5 = lerp(v, lerp(u, a8, a7), lerp(u, a6, a5)); // Her bliver 6 af vores linære interpolationer
      a4_1 = lerp(v, lerp(u, a4, a3), lerp(u, a2, a1)); // udregnet ved hjælp af funktionen Lerp().
                                                        // (findes også nedenunder).

      float result = ((lerp(w, a8_5, a4_1)+1)/2);   // Sidste interpolation udregnes.
                                                    // og værdien justeres fra intervallet
                                                    // [-1 - 1] til [0 - 1].
      
      return result; // resultat sendes retur til detn kaldte funktion
   }

    float fade(float t) // fadefunktionen søre for manipulation af vores 
   {                     // punkts lokale kordinator 
   
      return t * t * t * (t * (t * 6.0f - 15.0f) + 10.0f);
   }

    float lerp(float t, float a, float b) // Lerp() er funktionen
   {                                      // for linær interpolation.
      return a + t * (b - a); // linær interpolation
   }

    float grad(int hash, float x, float y, float z)
   {
        switch(hash & 0xF) // Tager vores sidste 4 bit af vores 8bit
                        // has værdi som er vores hjørners værdi.
                        // vi bruger et Switch case statement
                        // hvert tilfælde kan give vores prikprodukt
                        // tilbage. //bit flipping teknink er brugt//.
        // Orginal implementation er af Riven
        {
        case 0x0: return  x + y;
        case 0x1: return -x + y;
        case 0x2: return  x - y;
        case 0x3: return -x - y;
        case 0x4: return  x + x;
        case 0x5: return -x + x;
        case 0x6: return  x - x;
        case 0x7: return -x - x;
        case 0x8: return  y + x;
        case 0x9: return -y + x;
        case 0xA: return  y - x;
        case 0xB: return -y - x;
        case 0xC: return  y + z;
        case 0xD: return -y + x;
        case 0xE: return  y - x;
        case 0xF: return -y - z;
        default: return 0; // never happens
        }
    }
}

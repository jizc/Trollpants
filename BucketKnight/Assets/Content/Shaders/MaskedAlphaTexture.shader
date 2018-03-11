Shader "MaskedAlphaTexture"
{
    Properties
    {
        _MainTex ("Base (RGB) Trans. (Alpha)", 2D) = "white" { }
		
		_Mask ("Culling Mask", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range (0,1)) = 0.1
    }

    Category
    {
        ZWrite Off
        Alphatest Greater 0.5
        Cull Off
        SubShader
        {
            Pass
            {
                Lighting Off
				SetTexture [_Mask] {}
                SetTexture [_MainTex] {Combine texture * texture, previous * texture}
            }
        } 
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
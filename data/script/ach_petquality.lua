--�佫Ʒ�ʸ��� ������
x200005_g_scriptId = 200005


function x200005_OnCounter(uuid, eventId, quality,IsUp)
	--Ʒ������
	--enum QualityType
	--��
	--WhiteQuality	= 0;
	--��
	--GreenQuality	= 1;
	--��
	--BlueQuality	= 2;
	--��
	--PurpleQuality	= 3;
	--��
	--OrangeQuality = 4;
	--��
	--GoldenQuality	= 5;
	--��
	--BlackQuality	= 6;	


	--�ο�AchievementKeyTable.csv
	
	
	local achKey = 34
	
	if IsUp==0 then
		if quality >= 1 then
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + 1)
		end
		if quality >= 2 then
		achKey = 35
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + 1)
		end
		if quality >= 3 then
		achKey = 36
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + 1)
		end
	elseif IsUp==1 then
		if quality >= 3 then
		achKey = 36
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + 1)
		elseif quality >= 2 then
		achKey = 35
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + 1)
		elseif quality >= 1 then
		achKey = 34
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + 1)
		end
	end
			local strMsg = string.format("x200005_OnCounter uuid=%d, eventId=%d, quality=%d", 
					uuid, eventId, quality)
		LuaWriteLog(strMsg)
  	return 	
end


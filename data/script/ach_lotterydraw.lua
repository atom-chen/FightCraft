--̽���齱�¼� ������
x200004_g_scriptId = 200004


function x200004_OnCounter(uuid, eventId, count, type)
	
	--�ο�AchievementKeyTable.csv
	local achKey = 52

	if count > 0 then
	    if type == 0 then
		    LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + count)
		elseif type == 1 then
		    achKey = 54
            LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + count)
		end
	end
	
	local strMsg = string.format("x200004_OnCounter uuid=%d, eventId=%d, count=%d, type=%d", 
					uuid, eventId, count, type)
	LuaWriteLog(strMsg)
	
  	return
  	
end


--������ʯ
x100007_g_scriptId = 100007

--�¼�����
function x100007_OnEventFilter(uuid, achEventId, count)

	if count ~= nil and tonumber(count) <= LuaGetAllGemsByLevel(uuid, 3) then
  		return 1
  	else
  		return 0
  	end
  	
end

--�ɾ���ɽ���
function x100007_OnAchievementProgress(uuid, reachNum)
  	return 1
end


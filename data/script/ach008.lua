--�弶��ʯ
x100008_g_scriptId = 100008

--�¼�����
function x100008_OnEventFilter(uuid, achEventId, count)

	if count ~= nil and tonumber(count) <= LuaGetAllGemsByLevel(uuid, 5) then
  		return 1
  	else
  		return 0
  	end
  	
end

--�ɾ���ɽ���
function x100008_OnAchievementProgress(uuid, reachNum)
  	return 1
end

